// migration should be performed in the project that contains the db connection, in this case CleanArchitecture.Data
// command to start the migration -> add-migration "migrationName" -> example: add-migration InitialMigration
// a filter with the migrations should be generated in the CleanArchitecture.Data project
// then we shall apply those migrations to the db, to do that lets execute the update-database command
// after the command is executed the tables should be created in the db
using CleanArchitecture.Domain;
using CleanArchitecture.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

/*
StreamerDbContext dbContext = new();

Streamer streamer = new()
{
    Name = "Netflix",
    Url = "https://www.netflix.com"
};

dbContext.Streamers.Add(streamer);

await dbContext.SaveChangesAsync();
*/

namespace CleanArchitecture.ConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {

            // StreamerDbContext dbContext = new StreamerDbContext();

            // await AddStreamer(dbContext);

            // await AddListOfVideos(dbContext);

            // await FilterStreamers(dbContext);

            // await QueryMethods(dbContext);

            // await QueryLinq(dbContext);

            // await TrackingAndNoTracking(dbContext);

            // await AddNewStreamerWithVideo(dbContext);

            // await AddNewStreamerWithVideoId(dbContext);

            // await AddNewActorWithVideo(dbContext);

            // await AddNewDirectorWithVideo(dbContext);

            // await MultipleEntitiesQuery(dbContext);

        }

        static async Task ShowStreamers(StreamerDbContext context)
        {

            List<Streamer> streamers = await context.Streamers.ToListAsync();

            foreach(Streamer streamer in streamers)
                Console.WriteLine($"Id: {streamer.Id}\nName: {streamer.Name}\nUrl: {streamer.Url}");

        }

        static async Task AddStreamer(StreamerDbContext dbContext)
        {

            await ShowStreamers(dbContext);

            Console.WriteLine("Type in the streamer name:");
            string name = Console.ReadLine();
            Console.WriteLine("Type in the streamer url");
            string url = Console.ReadLine();
            Streamer streamer = new Streamer()
            {
                Name = name,
                Url = url
            };

            dbContext.Streamers.Add(streamer);

            await dbContext.SaveChangesAsync();

        }

        static async Task AddListOfVideos(StreamerDbContext dbContext)
        {

            await ShowStreamers(dbContext);

            Console.WriteLine("Type in the streamer id");
            int streamerId = int.Parse(Console.ReadLine());

            List<string> videos = new List<string>()
            {
                "Cincerella",
                "Peter Pan & Wendy",
                "Hocus Pocus"
            };

            List<Video> listOfVideos = new List<Video>();

            foreach(string video in videos)
            {
                listOfVideos.Add(new Video() { Name = video, StreamerId = streamerId});
            }

            await dbContext.Videos.AddRangeAsync(listOfVideos);
            
            await dbContext.SaveChangesAsync();

        }

        static async Task FilterStreamers(StreamerDbContext dbContext)
        {
            await ShowStreamers(dbContext);

            Console.WriteLine("Type in the streamer name");
            string name = Console.ReadLine();

            // In C#, the ! symbol is the null-forgiving operator. It tells the compiler to treat the expression as non-nullable, even if the type would normally allow null.
            List<Streamer> streamers = await dbContext.Streamers.Where(streamer => streamer.Name!.Equals(name)).ToListAsync();

            if(streamers.Count == 0)
            {
                // first way to do it - with c# methods
                // streamers = await dbContext.Streamers.Where(streamer => streamer.Name!.Contains(name!)).ToListAsync();
                // second way to do it - with entity framework methods, that are closer to sql sintax
                // we use the flag %name% to indicate that it should search for Streamer.Name that partially matches the name
                streamers = await dbContext.Streamers.Where(streamer => EF.Functions.Like(streamer.Name!, $"%{name}%")).ToListAsync();
            }

            foreach (Streamer streamer in streamers)
                Console.WriteLine($"Id: {streamer.Id}\nName: {streamer.Name}\nUrl: {streamer.Url}");
        }

        static async Task QueryMethods(StreamerDbContext dbContext)
        {
            Console.WriteLine("Type in the character");
            string character = Console.ReadLine();
            // The FristAsync method will return the first occurrance, the first record that acomplish the condition
            // but if no record acomplish the condition it will trow an error and stop the program execution
            var firstAsync = await dbContext.Streamers.Where(streamer => streamer.Name!.Contains(character)).FirstAsync();
            // The FirstOrDefaultAsync method will do the same, but unlike the above method it will not throw an exception
            // when not no record acomplish the condition, it will instead return null.
            var firstOrDefaultAsync_v1 = await dbContext.Streamers.Where(streamer => streamer.Name.Contains(character))
                                                          .FirstOrDefaultAsync();
            // other way to do the same
            var firstOrDefaultAsync_v2 = await dbContext.Streamers.FirstOrDefaultAsync(streamer => streamer.Name!.Contains(character));

            // SingleAsync method will search for a unique value, if the value is not unique (there are more fields with
            // the same value) or no value was found it will throw an exception
            var singleAsync = await dbContext.Streamers.Where(streamer => streamer.Id == 4).SingleAsync();
            // Same as the above method but if no records are found it will return null, if the record is not unique it will throw an exception
            var singleOrDefaultAsync = await dbContext.Streamers.Where(streamer => streamer.Id == 1).SingleOrDefaultAsync();

            // This one does the same but more directly, and returns null if it was not found
            var result = await dbContext.Streamers.FindAsync(1);
            // Console.WriteLine($"Id: {streamer1.Id}\nName: {streamer1.Name}\nUrl: {streamer1.Url}");

        }

        static async Task QueryLinq(StreamerDbContext dbContext)
        {
            Console.WriteLine("Type in the streamer name");
            string streamerName = Console.ReadLine();

            List<Streamer> streamers = await (from streamer in dbContext.Streamers
                                       where EF.Functions.Like(streamer.Name!, $"%{streamerName}%")
                                       select streamer).ToListAsync();

            foreach(Streamer streamer in streamers)
            {
                Console.WriteLine($"Id: {streamer.Id}\nName: {streamer.Name}\nUrl: {streamer.Url}");
            }

        }

        static async Task TrackingAndNoTracking(StreamerDbContext dbContext)
        {
            // entity will be stay loaded in memory so any update to the entity will be possible
            var streamerTracking = await dbContext.Streamers.FindAsync(4);
            // entity will not stay loaded in memory so updates to the entity will not be possible
            var streamerNoTracking = await dbContext.Streamers.AsNoTracking().FirstOrDefaultAsync(streamer => streamer.Id == 5);

            // more info
            /*
             In Entity Framework, Tracking and NoTracking are related to how the DbContext keeps track of the entities it retrieves from the database.

             Tracking:

                When you retrieve an entity using methods like Find or FirstOrDefault, the entity is tracked by the DbContext. This means that any changes made to the entity will be detected by the DbContext, and those changes will be persisted to the database when you call SaveChanges.
                In your example, streamerTracking is retrieved using Find method, and by default, it is tracked by the DbContext.
             NoTracking:

                When you use AsNoTracking() method, it tells the DbContext not to track the entities that are retrieved. This can be useful in scenarios where you are only reading data and don't intend to modify or save changes to the database.
                In your example, streamerNoTracking is retrieved using AsNoTracking(), which means that any changes made to this entity will not be automatically tracked by the DbContext. If you modify streamerNoTracking and want to persist those changes, you would need to attach the entity to the DbContext and explicitly set its state to Modified before calling SaveChanges.
                In summary, the key difference lies in whether or not the DbContext keeps track of changes to the entities. Using Tracking is beneficial when you plan to modify and save changes to the entities, while NoTracking is useful when you are working with read-only scenarios or when you want to reduce the overhead of change tracking for certain queries. 
             */

            streamerTracking!.Name = "HBO MAX PLUS"; // this change is indeed saved to the database
            streamerNoTracking!.Name = "DISNEY PLUS"; // this change is not saved to database

            await dbContext.SaveChangesAsync();

        }

        static async Task AddNewStreamerWithVideo(StreamerDbContext dbContext)
        {

            Streamer pantaya = new Streamer() { Name = "Pantaya" };
            Video hungerGames = new Video() { Name = "Hunger Games", Streamer = pantaya };

            // start tracking the given entity and any other recheable entity, this means that if the entity hungerGames has other
            // entity attached to it that entity will be tracked as well, and as we can see hungerGames does have a entity attached to it
            // it is pantaya, so when SaveChangesAsync method is called both entities are going to be saved and the Streamer id will be
            // inserted in the Video class and table StremerId field.
            await dbContext.AddAsync(hungerGames);
            await dbContext.SaveChangesAsync();

        }

        static async Task AddNewStreamerWithVideoId(StreamerDbContext dbContext)
        {
            Video batmanForever = new Video() { Name = "Batman Forever", StreamerId = 1002 };

            await dbContext.AddAsync(batmanForever);
            await dbContext.SaveChangesAsync();
        }

        static async Task AddNewActorWithVideo(StreamerDbContext dbContext)
        {
            Actor actor = new Actor() { Name = "Brad", LastName = "Pitt" };

            await dbContext.AddAsync(actor);
            await dbContext.SaveChangesAsync();

            VideoActor videoActor = new VideoActor() { ActorId = actor.Id, VideoId = 1 };

            await dbContext.AddAsync(videoActor);
            await dbContext.SaveChangesAsync();

        }

        static async Task AddNewDirectorWithVideo(StreamerDbContext dbContext)
        {

            Director director = new Director() { Name = "Lorenzo", LastName = "Basteri", VideoId = 1};
            await dbContext.AddAsync(director);
            await dbContext.SaveChangesAsync();

        }

        static async Task MultipleEntitiesQuery(StreamerDbContext dbContext)
        {
            /*Video videoWithActors = await dbContext.Videos.Include(e => e.Actors).FirstOrDefaultAsync(e => e.Id == 1);

            Console.WriteLine($"Id: {videoWithActors.Id}\nName: {videoWithActors.Name}");
            foreach(Actor actor in videoWithActors.Actors)
            {
                Console.WriteLine($"Id: {actor.Id}\nName: {actor.Name}\nLast Name: {actor.LastName}");
            }*/

            // var actors = await dbContext.Actors.Select(a => a.Name).ToListAsync(); // retrieving just one property
            /*var actors = await dbContext.Actors.Select(a => new {a.Name, a.Id}).ToListAsync();

            foreach(var actorName in actors)
            {
                Console.WriteLine(actorName.Id);
                Console.WriteLine(actorName.Name);
            }*/

            /*var videoWithDirector = await dbContext.Videos.Include(v => v.Director)
                                                          .Select(v => new
                                                          {
                                                              directorFullName = $"{v.Director.Name} {v.Director.LastName}",
                                                              movie = v.Name
                                                          })
                                                          .ToListAsync();*/
            var videoWithDirector = await dbContext.Videos.Include(v => v.Director)
                // it is recommended to add the condition to the main entity in the query, in this case that is Videos
                                                          .Where(v => v.Director != null)
                                                          .Select(v => new
                                                          {
                                                              directorFullName = $"{v.Director.Name} {v.Director.LastName}",
                                                              movie = v.Name
                                                          })
                                                          .ToListAsync();
            foreach (var video in videoWithDirector)
            {
                Console.WriteLine($"Movie: {video.movie}\nDirector: {video.directorFullName}");
            }

        }

    }
}