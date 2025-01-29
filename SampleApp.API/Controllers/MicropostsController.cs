using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SampleApp.API.Dto;
using SampleApp.API.Entities;
using SampleApp.API.Repositories;
using SampleApp.Repositories;

namespace SampleApp.API.Controllers;

public class MicropostsController(BaseRepository<Micropost> repo, IMapper mapper) : BaseController<Micropost>(repo)
{

    [HttpGet("users")]
    public async Task<ActionResult<PagedResult<MicropostDto>>> GetAllWithUser()
    {

        var result = await repo.GetAllAsync(
                            filter: x => x.Id >= 1,
                            orderBy: q => q.OrderBy(x => x.Content),
                            include: q => q.Include(x => x.User),
                            pageNumber: 1,
                            pageSize: 2
                        );

        return new PagedResult<MicropostDto>
        {
            Items = mapper.Map<List<MicropostDto>>(result.Items),
            TotalCount = result.TotalCount,
            PageNumber = result.PageNumber,
            PageSize = result.PageSize
        };
    }

}