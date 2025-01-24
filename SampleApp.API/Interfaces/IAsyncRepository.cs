namespace SampleApp.API.Interfaces;

public interface IAsyncRepository<T> where T : class
{
    Task<T> GetAsync(int id);
    Task Create(T entity);
    Task<T> Update(T entity);
    Task<bool> DeleteAsync(int id);
}