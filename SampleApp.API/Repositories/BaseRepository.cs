using Microsoft.EntityFrameworkCore;
using SampleApp.API.Data;
using SampleApp.API.Entities;
using SampleApp.API.Interfaces;

namespace SampleApp.Repositories;

public class BaseRepository<T>(SampleAppContext db) : IAsyncRepository<T> where T: class
{

    public async Task Create(T entity)
    {
        try{
            db.Add(entity);
            await db.SaveChangesAsync();
        }
        catch(Exception e){
            throw new Exception(e.InnerException!.Message);
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        db.Remove(await Find(id));
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
       return entity ?? throw new Exception($"Not Found {nameof(T)} c id = {id}");
    }

    public async Task<T> Update(T entity)
    {
        db.Update(entity);
        await db.SaveChangesAsync();
        return entity;
    }

    public async Task<IEnumerable<T>> GetAll()
    {
        return await db.Set<T>().Include(t => t.Roles).ToListAsync();
    }


    public Task Create(Micropost entity)
    {
        throw new NotImplementedException();
    }

    public Task<Micropost> Update(Micropost entity)
    {
        throw new NotImplementedException();
    }

}
