using CleanArchitecture.Domain;

namespace CleanArchitecture.Application.Specifications.Streamers
{
    // implementing specification for a specific object
    public class StreamersWithVideosSpecification : BaseSpecification<Streamer>
    {
        public StreamersWithVideosSpecification()
        {
            AddInclude(s => s.Videos!);
        }

        public StreamersWithVideosSpecification(string url) : base(s => s.Url!.Contains(url))
        {
            AddInclude(s => s.Videos!);
        }
    }
}
