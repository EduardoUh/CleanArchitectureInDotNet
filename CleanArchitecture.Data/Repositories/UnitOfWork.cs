using CleanArchitecture.Application.Contracts.Persistence;
using CleanArchitecture.Domain.Common;
using CleanArchitecture.Infrastructure.Persistence;
using System.Collections;

namespace CleanArchitecture.Infrastructure.Repositories
{
    public class UnitOfWork(StreamerDbContext context) : IUnitOfWork
    {
        private Hashtable _repositories;
        private readonly StreamerDbContext _context = context;

        // using unit of work with custom repositories
        private IVideoRepository _videoRepository;
        private IStreamerRepository _streamerRepository;

        public IVideoRepository VideoRepository => _videoRepository ??= new VideoRepository(_context);
        public IStreamerRepository StreamerRepository => _streamerRepository ??= new StreamerRepository(_context);

        public StreamerDbContext Context => _context;

        // Complete method is in charge of trigger the confirmation of all the transactions you are doing
        // not the each repository reference
        public async Task<int> Complete()
        {
            return await _context.SaveChangesAsync();
        }

        // context will be deleted after the transaction ends
        public void Dispose()
        {
            _context.Dispose();// releases the allocated resources for this context
        }

        public IAsyncRepository<TEntity> Repository<TEntity>() where TEntity : BaseDomainModel
        {
            // if repositories is null, then create it
            if (_repositories == null)
            {
                _repositories = new Hashtable();
            }
            // other approach using compound assignment
            /*
             _repositories ??= new Hashtable();
             */

            // getting the name of the entity
            var type = typeof(TEntity).Name;

            // if entity is not in the repositories collection then add it
            if (!_repositories.ContainsKey(type))
            {
                // setting the type, we use this because all of our repositories implements heritages from this class
                var repositoryType = typeof(RepositoryBase<>);
                // creating a instance of the repository using reflection
                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), _context);
                // adding the repository instance to the repositories collection
                //                  key     value
                _repositories.Add(type, repositoryInstance);
            }
            /*
             returning the repository for the specified entity type. The type variable is used to look up the repository 
             instance in the collection, and it's cast to IAsyncRepository<TEntity> before being returned.
             */
            return (IAsyncRepository<TEntity>)_repositories[type];
        }

    }
}
