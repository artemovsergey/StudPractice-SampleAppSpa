using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using SampleApp.API.Data;
using SampleApp.API.Interfaces;
using SampleApp.API.Repositories;

namespace SampleApp.Repositories;

public class BaseRepository<T>(SampleAppContext db) : IAsyncRepository<T> where T : class, IEntity
{

    public async Task<T> Create(T entity)
    {
        try
        {
            db.Add(entity);
            await db.SaveChangesAsync();
            return entity;
        }
        catch (Exception e)
        {
            throw new Exception(e.InnerException!.Message);
        }
    }

    public async Task<bool> DeleteAsync(T entity)
    {
        db.Remove(await Find(entity.Id));
        await db.SaveChangesAsync();
        return true;
    }

    public async Task<T> GetAsync(int id)
    {
        return await Find(id);
    }

    public virtual async Task<T> Find(int id)
    {
        var entity = await db.Set<T>().FindAsync(id);
        return entity ?? throw new Exception($"Not Found {typeof(T).ShortDisplayName()} c id = {id}");
    }

    public async Task<T> Update(T entity)
    {
        db.Update(entity);
        await db.SaveChangesAsync();
        return entity;
    }

    public async Task<IEnumerable<T>> GetAll(
            Func<IQueryable<T>, IQueryable<T>>? include = null)
    {
        var query = db.Set<T>().AsQueryable();

        if (include != null)
        {
            query = include(query); // Применяем переданные Includes
        }

        return await query.ToListAsync();
    }

    public Task<bool> DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }


    public virtual async Task<IEnumerable<T>> GetAllAsync(
        Expression<Func<T, bool>>? filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        Func<IQueryable<T>, IQueryable<T>>? include = null,
        int? pageNumber = null,
        int? pageSize = null)
    {
        var query = db.Set<T>().AsQueryable();

        if (include != null)
            query = include(query);

        if (filter != null)
            query = query.Where(filter);

        if (orderBy != null)
            query = orderBy(query);

        if (pageNumber.HasValue && pageSize.HasValue)
            query = query.Skip((pageNumber.Value - 1) * pageSize.Value).Take(pageSize.Value);

        return await query.ToListAsync();
    }

    public async Task<PagedResult<T>> GetAllAsync(
        Expression<Func<T, bool>>? filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        Func<IQueryable<T>, IQueryable<T>>? include = null,
        int pageNumber = 1,
        int pageSize = 10)
    {
        if (pageNumber < 1) throw new ArgumentOutOfRangeException(nameof(pageNumber));
        if (pageSize < 1) throw new ArgumentOutOfRangeException(nameof(pageSize));

        var query = db.Set<T>().AsQueryable();

        if (include != null)
            query = include(query);

        if (filter != null)
            query = query.Where(filter);

        var totalCount = await query.CountAsync();

        query = orderBy != null 
            ? orderBy(query) 
            : query.OrderBy(e => e.Id); // Сортировка по умолчанию

        query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);

        return new PagedResult<T>
        {
            Items = await query.ToListAsync(),
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
    }

}

public interface IEntity
{
    public int Id { get; set; }
}