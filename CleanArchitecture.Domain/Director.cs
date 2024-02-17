using CleanArchitecture.Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace CleanArchitecture.Domain
{
    public class Director : BaseDomainModel
    {
        public string? Name { get; set; }
        public string? LastName { get; set; }
        // this is called caltulated field, it doesn't exist in the db but it is calculated based on db data on execution time
        // like this one, you shall add the NotMapped property to indicate to EF to not include it as a column in the
        // Director table
        [NotMapped]
        public string? FullName => $"{Name} {LastName}";
        public virtual ICollection<Video>? Videos { get; set; }
    }
}
