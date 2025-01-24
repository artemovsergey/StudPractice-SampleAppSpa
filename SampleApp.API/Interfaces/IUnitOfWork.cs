namespace SampleApp.API.Interfaces;

public interface IUnitOfWork
{
    public Task CommitChangedAsync(CancellationToken cancellationToken = default);
}
