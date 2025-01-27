using Microsoft.AspNetCore.Mvc;
using SampleApp.API.Entities;
using SampleApp.Repositories;

namespace SampleApp.API.Controllers;

public class MicropostsController(BaseRepository<Micropost> repo) : BaseController<Micropost>(repo)
{

    [HttpGet("test")]
    public async Task TestMethod(){
        await GetAll();
    }
}