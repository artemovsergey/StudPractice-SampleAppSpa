using Microsoft.AspNetCore.Mvc;
using SampleApp.API.Entities;
using SampleApp.API.Interfaces;
using SampleApp.API.Validations;

namespace SampleApp.API.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase

{
    private readonly IUserRepository _repo;
    public UsersController(IUserRepository repo)

    {
       _repo = repo;
    }

    [HttpPost]
    public ActionResult CreateUser(User user){

        var validator = new FluentValidator();
        var result = validator.Validate(user);
        if(!result.IsValid){
            throw new Exception($"{result.Errors.First().ErrorMessage}");
        }

        return Ok(_repo.CreateUser(user));

    }
    
    [HttpGet]
    public ActionResult GetUser(){
        return Ok(_repo.GetUsers());
    }
    

    [HttpPut]
    public ActionResult UpdateUser(User user){
       return Ok(_repo.EditUser(user, user.Id));
    }


    [HttpGet("{id}")]
    public ActionResult GetUserById(int id){
       return Ok(_repo.FindUserById(id));
    }


    [HttpDelete]
    public ActionResult DeleteUser(int id){
        return Ok(_repo.DeleteUser(id));
    }

}