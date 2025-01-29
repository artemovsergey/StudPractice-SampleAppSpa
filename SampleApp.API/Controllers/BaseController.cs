using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SampleApp.Repositories;

namespace SampleApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BaseController<T>(BaseRepository<T> repo) : ControllerBase where T : class, IEntity
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<T>>> GetAll(){
        return Ok(await repo.GetAll());
    }

    [HttpPost]
    public async Task<ActionResult<T>> Create(T t){
        return Created("", await repo.Create(t));
    }

    [HttpGet("id")]
    public async Task<ActionResult<T>> Get(int id){
        return Ok(await repo.Find(id));
    }

    [HttpPut]
    public async Task<ActionResult<T>> Update(T t){
        return Ok(await repo.Update(t));
    }

    [HttpDelete]
    public async Task<ActionResult<T>> Delete(T t){
        return Ok(await repo.DeleteAsync(t));
    }
}