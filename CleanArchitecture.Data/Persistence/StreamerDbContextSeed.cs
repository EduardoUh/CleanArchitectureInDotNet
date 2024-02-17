using CleanArchitecture.Domain;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Infrastructure.Persistence
{
    public class StreamerDbContextSeed
    {
        // we changed from ILogger to ILogerFactory because the las one can be invoked from the main program of the web api
        public static async Task SeedAsync(StreamerDbContext context, ILoggerFactory loggerFactory)
        {
            if (!context.Streamers.Any())
            {
                var logger = loggerFactory.CreateLogger<StreamerDbContextSeed>();
                context.Streamers.AddRange(GetPreconfiguredStreamer());
                await context.SaveChangesAsync();
                logger.LogInformation("Saving new records to db {context}", typeof(StreamerDbContext).Name);
            }
        }

        private static IEnumerable<Streamer> GetPreconfiguredStreamer()
        {
            return new List<Streamer>
            {
                new Streamer{CreatedBy = "Eduardo", Name = "Maxi HBP", Url = "http://www.hbp.com"},
                new Streamer{CreatedBy = "Eduardo", Name = "Amazon VIP", Url = "http://www.amazonvip.com"},
            };
        }

    }
}
