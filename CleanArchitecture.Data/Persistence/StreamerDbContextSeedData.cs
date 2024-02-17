using CleanArchitecture.Domain;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace CleanArchitecture.Infrastructure.Persistence
{
    public class StreamerDbContextSeedData
    {
        public static async Task LoadDataAsync(StreamerDbContext context, ILoggerFactory loggerFactory)
        {
            try
            {
                var videos = new List<Video>();

                if(!context.Directors.Any())
                {
                    // we use the Data folder because we changed the project name to Infrastructure not the directory name
                    // you can go an check it out.
                    var directorsData = File.ReadAllText("../CleanArchitecture.Data/Data/director.json");
                    var directors = JsonSerializer.Deserialize<List<Director>>(directorsData);
                    await context.Directors!.AddRangeAsync(directors!);
                    await context.SaveChangesAsync();
                }

                if (!context.Videos.Any())
                {
                    // we use the Data folder because we changed the project name to Infrastructure not the directory name
                    // you can go an check it out.
                    var videosData = File.ReadAllText("../CleanArchitecture.Data/Data/video.json");
                    videos = JsonSerializer.Deserialize<List<Video>>(videosData);
                    // video needs a reference to a director, we do that in this method
                    await GetPreconfiguredVideoDirectorAsync(videos!, context);
                    await context.SaveChangesAsync();
                }

                if (!context.Actors.Any())
                {
                    // we use the Data folder because we changed the project name to Infrastructure not the directory name
                    // you can go an check it out.
                    var actorsData = File.ReadAllText("../CleanArchitecture.Data/Data/actor.json");
                    var actors = JsonSerializer.Deserialize<List<Actor>>(actorsData);
                    await context.Actors.AddRangeAsync(actors!);
                    // remember that we have a VideoActor table, and now that we have videos and actors lists we can
                    // save that data to the VideoActor table in the db
                    await context.AddRangeAsync(GetPreconfiguredVideoActor(videos!));
                    await context.SaveChangesAsync();
                }

            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<StreamerDbContextSeedData>();

                logger.LogError(ex.Message);
            }
        }

        private static async Task GetPreconfiguredVideoDirectorAsync(ICollection<Video> videos, StreamerDbContext context)
        {
            var random = new Random();
            foreach (var video in videos)
            {
                // at this moment we should have inserted the 100 directors data to the db, thats why we set the range
                // from 1 to 99, because we have 100 directors
                video.DirectorId = random.Next(1, 99);
            }

            await context.Videos.AddRangeAsync(videos);
        }

        private static IEnumerable<VideoActor> GetPreconfiguredVideoActor(ICollection<Video> videos)
        {
            var videoActors = new List<VideoActor>();
            var random = new Random();

            foreach(var video in videos)
            {
                videoActors.Add(new VideoActor
                {
                    VideoId = video.Id,
                    ActorId = random.Next(1, 99)
                });
            }

            return videoActors;
        }

    }
}
