using CleanArchitecture.Application.Contracts.Persistence;
using CleanArchitecture.Domain;
using CleanArchitecture.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infrastructure.Repositories
{
    public class VideoRepository(StreamerDbContext context) : RepositoryBase<Video>(context), IVideoRepository
    {
        public async Task<Video> GetVideoByTitle(string videoTitle)
        {
            return await _context.Videos.Where(v => v.Name == videoTitle).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Video>> GetVideoByUserName(string userName)
        {
            return await _context.Videos.Where(v => v.CreatedBy!.Equals(userName)).ToListAsync();
        }
    }
}
