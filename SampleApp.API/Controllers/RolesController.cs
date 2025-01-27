using Microsoft.AspNetCore.Mvc;
using SampleApp.API.Entities;
using SampleApp.Repositories;

namespace SampleApp.API.Controllers;

public class RolesController(BaseRepository<Role> repo) : BaseController<Role>(repo)
{
}
