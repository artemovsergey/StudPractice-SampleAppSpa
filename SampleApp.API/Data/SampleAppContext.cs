using Microsoft.EntityFrameworkCore;
using SampleApp.API.Entities;

namespace SampleApp.API.Data;

public class SampleAppContext : DbContext
{
    public DbSet<User> Users { get; set; }

    public SampleAppContext()
    {
        Database.EnsureCreated();
    }

    public SampleAppContext(DbContextOptions<SampleAppContext> opt) : base(opt)
    {
        
    }
}