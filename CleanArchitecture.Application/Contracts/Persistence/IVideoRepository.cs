using CleanArchitecture.Domain;

// create a interface like this when you need something more specific than the IAsyncRepository
// in this case we need something more specific for the Video class or Videos entity
namespace CleanArchitecture.Application.Contracts.Persistence
{
    // now the IVideoRepository heritages all the IAsyncRepository generic methods and you can add more specific methods that will
    // be exclusive for IVideoRepository
    public interface IVideoRepository : IAsyncRepository<Video>
    {
        Task<Video> GetVideoByTitle(string videoTitle);
        Task<IEnumerable<Video>> GetVideoByUserName(string userName);
    }
}
