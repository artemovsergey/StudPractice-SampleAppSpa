using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SampleApp.API.Dto;
using SampleApp.API.Entities;
using SampleApp.API.Interfaces;
using SampleApp.API.Validations;

namespace SampleApp.API.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserRepository _repo;
    private readonly ITokenService _tokenService;
    public UsersController(IUserRepository repo, ITokenService tokenService)
    {
       _tokenService = tokenService;
       _repo = repo;
    }

    [Authorize]
    [HttpPost]
    public ActionResult CreateUser(UserDto userDto){

        using var hmac = new HMACSHA512();
        
        var user = new User(){
           Login = userDto.Login,
           PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(userDto.Password)),
           PasswordSalt = hmac.Key,
           Token = _tokenService.CreateToken(userDto.Login)
        };


        var validator = new FluentValidator();
        var result = validator.Validate(user);
        if(!result.IsValid){
            throw new Exception($"Ошибка: {result.Errors.First().ErrorMessage}");
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