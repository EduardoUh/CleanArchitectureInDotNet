using AutoFixture;
using CleanArchitecture.Domain;
using CleanArchitecture.Infrastructure.Persistence;

namespace CleanArchitecture.Application.UnitTests.Mocks
{
    public static class MockStreamerRepository
    {
        public static void AddDataStreamerRepository(StreamerDbContext streamerDbContextFake)
        {
            // creating fake data
            var fixture = new Fixture();

            // we add this to avoid issues with circular reference when running the tests
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            var streamers = fixture.CreateMany<Streamer>().ToList();

            // adding a specific record
            streamers.Add(fixture.Build<Streamer>()
                // this will create streamer data including videos because both entities have a relation
                .With(streamer => streamer.Id, 800)
                // but we don't want video data to be created because that will cause issues, so we must
                // state that we don't want videos data
                .Without(streamer => streamer.Videos)
                .Create()
                );

            // adding created data to the db (in memory)
            streamerDbContextFake.Streamers.AddRange(streamers);
            // saving changes
            streamerDbContextFake.SaveChanges();
        }
    }
}
