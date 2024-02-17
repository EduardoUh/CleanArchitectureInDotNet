using CleanArchitecture.Application.Specifications;
using CleanArchitecture.Domain.Common;
using System.Linq.Expressions;

namespace CleanArchitecture.Application.Contracts.Persistence
{
    // here we are creating a interface that receives a generic type, but that type should have the
    // implementation of the BaseDomainModel
    public interface IAsyncRepository<T> where T : BaseDomainModel
    {
        Task<IReadOnlyList<T>> GetAllAsync(); // method to get all data
        /*
        Remember that link is a tool that provides a useful sintaxis to query data from collections, 
        databases, xml files, etc.
        Difference between Funct<T> and Expression<Func<T>> is that the first is called in the type 
        IEnumerable(data in memory ram) and the second one is called in the type IQueryable (remote date
        like database, xml)
        it would look like this internally:
            IEnumerable -> Where(Func<T, bool> predicate)
            IQueryable  -> Where(Expression<Func<T, bool>> predicate)
        but you will se this for both -> Where(x => x.property == "value")
        Their relationship with delegates is obvious, we know that delegates allow us to use methods as
        variables so we can pass them as parameters.
        .net developers use Expression to represent the query conditions that will turn into sql syntax
        this is interesting because expressions can be dynamic and variables allowing us to create the
        complex queries that our project needs.
        In conclusion, when you want to work with local data in the temporal memory use Func<T> and 
        when you want to work with remote data use Expression<Func<T>>
         */
        Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate); // method to get data based in
                                                                              // a expression
        Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate = null,
                                        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                        string includeString = null,
                                        bool disableTracking = true); // method to get data based in a
                                                                      // expression but allows
                                                                      // sorting and to include another entity
        Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate = null,
                                        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                        List<Expression<Func<T, object>>> includes = null,
                                        bool disableTracking = true); // method to get data based in a
                                                                      // expression but allows
                                                                      // to include a list of entities
        Task<T> GetByIdAsync(int id); // method to get data based in the id

        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task DeleteAsync(T entity);

        void AddEntity(T entity);
        void UpdateEntity(T entity);
        void DeleteEntity(T entity);

        // specification pattern
        Task<T> GetByIdWithSpec(ISpecification<T> spec);
        Task<IReadOnlyList<T>> GetAllWithSpec(ISpecification<T> spec);
        Task<int> CountAsync(ISpecification<T> spec);
    }
}
