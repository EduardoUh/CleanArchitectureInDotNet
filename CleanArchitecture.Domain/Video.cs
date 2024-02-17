using CleanArchitecture.Domain.Common;

namespace CleanArchitecture.Domain
{
    public class Video : BaseDomainModel
    {

        public Video()
        {
            Actors = new HashSet<Actor>();
        }
        public string? Name { get; set; } // = string.Empty; // we can also set the default value to be an emtpy string

        public int StreamerId { get; set; }

        // we need an ancle that represents the entity
        // this will be overrided by the entity stremer that is attached to the StreamerId
        // thats is why we set it to be type Streamer
        public virtual Streamer? Streamer { get; set; }
        public int DirectorId { get; set; }
        public virtual Director? Director { get; set; }
        // Each video can have several actors, so we need to create that property
        public virtual ICollection<Actor>? Actors { get; set; }
        public virtual ICollection<VideoActor>? VideoActors { get; set; }
    }
}
