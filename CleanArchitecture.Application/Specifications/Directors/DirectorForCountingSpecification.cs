using CleanArchitecture.Domain;

namespace CleanArchitecture.Application.Specifications.Directors
{
    public class DirectorForCountingSpecification : BaseSpecification<Director>
    {
        public DirectorForCountingSpecification(DirectorSpecificationParams directorParams)
            : base(
                  x => string.IsNullOrEmpty(directorParams.Search) || x.Name!.Contains(directorParams.Search)
                  )
        { }
    }
}
