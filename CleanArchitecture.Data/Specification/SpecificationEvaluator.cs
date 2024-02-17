using CleanArchitecture.Application.Specifications;
using CleanArchitecture.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infrastructure.Specification
{
    public class SpecificationEvaluator<T> where T : BaseDomainModel
    {
        public static IQueryable<T> GetQuery(IQueryable<T> inputQuery, ISpecification<T> spec)
        { 
            if(spec.Criteria != null)
            {
                // Where method filters a sequence of values based on a predicate
                inputQuery = inputQuery.Where(spec.Criteria);
            }

            if(spec.OrderBy !=null)
            {
                inputQuery.OrderBy(spec.OrderBy);
            }

            if(spec.OrderByDescending !=null)
            {
                inputQuery.OrderByDescending(spec.OrderByDescending);
            }

            if (spec.IsPaginationEnabled)
            {
                inputQuery = inputQuery.Skip(spec.Skip).Take(spec.Take);
            }

            inputQuery = spec.Includes.Aggregate(inputQuery, (current, include) => current.Include(include));

            return inputQuery;
        }
    }
}
