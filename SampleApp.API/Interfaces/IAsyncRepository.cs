using System.Linq.Expressions;
using SampleApp.API.Repositories;

namespace SampleApp.API.Interfaces;

public interface IAsyncRepository<T> where T : class
{
    Task<T> GetAsync(int id);
    Task<T> Create(T entity);
    Task<T> Update(T entity);
    Task<bool> DeleteAsync(int id);

    Task<PagedResult<T>> GetAllAsync(
        Expression<Func<T, bool>>? filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        Func<IQueryable<T>, IQueryable<T>>? include = null,
        int pageNumber = 1,
        int pageSize = 10);


}