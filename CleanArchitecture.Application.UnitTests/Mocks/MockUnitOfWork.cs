using CleanArchitecture.Infrastructure.Persistence;
using CleanArchitecture.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace CleanArchitecture.Application.UnitTests.Mocks
{
    // goal of this class is to represent the different repositories instances (Videos, Streamers, Directors)
    public static class MockUnitOfWork
    {
        public static Mock<UnitOfWork> GetUnitOfWork()
        {
            Guid dbContextId = Guid.NewGuid();

            // it is recommended to use the same db structure you use in your project, so we gonna use EF here but in memory
            var options = new DbContextOptionsBuilder<StreamerDbContext>()
                .UseInMemoryDatabase(databaseName: $"StreamerDbContext-{dbContextId}")
                .Options;

            var streamerDbContextFake = new StreamerDbContext(options);

            // ensuring that db is clean before running tests
            streamerDbContextFake.Database.EnsureDeleted();

            var mockUnitOfWork = new Mock<UnitOfWork>(streamerDbContextFake);
            
            return mockUnitOfWork;
        }
    }
}
