using SampleApp.API.Data;
using SampleApp.API.Interfaces;

namespace SampleApp.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly SampleAppContext _db;
    public UnitOfWork(SampleAppContext db)
    {
        _db = db;
    }

    public async Task CommitChangedAsync(CancellationToken cancellationToken)
    {
        try
        {
            await _db.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }

    }
}