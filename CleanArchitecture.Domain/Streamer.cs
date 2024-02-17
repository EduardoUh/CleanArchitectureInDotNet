using CleanArchitecture.Domain.Common;

namespace CleanArchitecture.Domain
{
    public class Streamer : BaseDomainModel
    {

        // by using the ? sign we indicate that the property can be nullable in the db, that will be
        // set when the class is mapped to the db
        public string? Name { get; set; }
        public string? Url { get; set; }
        // Video class has a reference to the Streamer so we can se that the videos are bound to a streamer, so it is logic to
        // create a reference to the collection of videos that are bound to the streamer.
        public ICollection<Video>? Videos { get; set; }

    }
}
