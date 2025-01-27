using Microsoft.AspNetCore.Mvc;
using SampleApp.Repositories;

namespace SampleApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BaseController<T>(BaseRepository<T>  repo) : ControllerBase where T : class
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<T>>> GetAll(){
        return Ok(await repo.GetAll());
    }
}