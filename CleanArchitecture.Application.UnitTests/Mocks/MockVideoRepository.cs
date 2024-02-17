using AutoFixture;
using CleanArchitecture.Domain;
using CleanArchitecture.Infrastructure.Persistence;

namespace CleanArchitecture.Application.UnitTests.Mocks
{
    public static class MockVideoRepository
    {
        public static void AddDataVideoRepository(StreamerDbContext streamerDbContextFake)
        {
            // creating fake data
            var fixture = new Fixture();

            // we add this to avoid issues with circular reference when running the tests
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            var videos = fixture.CreateMany<Video>().ToList();

            // adding a specific record
            videos.Add(fixture.Build<Video>()
                .With(video => video.CreatedBy, "Eduardo")
                .Create()
                );

            // adding created data to the db (in memory)
            streamerDbContextFake.Videos.AddRange(videos);
            // saving changes
            streamerDbContextFake.SaveChanges();
        }
    }
}
