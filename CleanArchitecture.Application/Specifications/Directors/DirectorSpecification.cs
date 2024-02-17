using CleanArchitecture.Domain;

namespace CleanArchitecture.Application.Specifications.Directors
{
    public class DirectorSpecification : BaseSpecification<Director>
    {
        public DirectorSpecification(DirectorSpecificationParams directorParams)
            : base(
                  x =>
                  string.IsNullOrEmpty(directorParams.Search) || x.Name!.Contains(directorParams.Search)
                  )
        {
            ApplyPagination(directorParams.PageSize, directorParams.PageSize * (directorParams.PageIndex - 1));

            if (!string.IsNullOrEmpty(directorParams.Sort))
            {
                switch (directorParams.Sort)
                {
                    case "nameAscending":
                        AddOrderBy(d => d.Name!);
                        break;
                    case "nameDescending":
                        AddOrderByDescending(d => d.Name!);
                        break;
                    case "LastNameAscending":
                        AddOrderBy(d => d.LastName!);
                        break;
                    case "LastNameDescending":
                        AddOrderByDescending(d => d.LastName!);
                        break;
                    case "CreationDateAscending":
                        AddOrderBy(d => d.CreationDate);
                        break;
                    case "CreationDateDescending":
                        AddOrderByDescending(d => d.CreationDate);
                        break;
                    default:
                        AddOrderBy(d => d.Name!);
                        break;
                }
            }
        }

    }
}
