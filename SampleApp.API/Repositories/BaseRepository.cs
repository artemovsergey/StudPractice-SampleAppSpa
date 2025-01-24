using SampleApp.API.Data;
using SampleApp.API.Interfaces;

namespace SampleApp.Infrastructure.Repositories;

public class BaseRepository<T> : IAsyncRepository<T> where T : class
{

    protected readonly SampleAppContext _db;
    public BaseRepository(SampleAppContext db)
    {
        _db = db;
    }

    public Task Create(T entity)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<T> GetAsync(int id)
    {
        throw new NotImplementedException();
    }

    // public virtual async Task<T> GetByIdAsync(int id)
    // {
    //    return await _db.Set<T>().FindAsync(id);
    // }

    public Task<T> Update(T entity)
    {
        throw new NotImplementedException();
    }
}
