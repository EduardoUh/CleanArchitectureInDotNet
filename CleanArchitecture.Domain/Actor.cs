using CleanArchitecture.Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace CleanArchitecture.Domain
{
    public class Actor : BaseDomainModel
    {

        public Actor()
        {
            // initializing the Videos property
            Videos = new HashSet<Video>();
        }
        public string? Name { get; set; }
        public string? LastName { get; set; }
        [NotMapped]
        public string? FullName => $"{Name} {LastName}";
        // The Actor is bound to a collection of videos, so we need to create this property
        public virtual ICollection<Video> Videos { get; set; }
        public virtual ICollection<VideoActor>? VideoActors { get; set; }
    }
}
