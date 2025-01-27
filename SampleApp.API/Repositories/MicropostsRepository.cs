using SampleApp.API.Data;
using SampleApp.API.Entities;
using SampleApp.Repositories;

namespace SampleApp.API.Repositories;

public class MicropostsRepository(SampleAppContext db) : BaseRepository<Micropost>(db)
{
    
}