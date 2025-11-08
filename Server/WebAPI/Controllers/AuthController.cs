using ApiContracts.DTOs;
using Entities;
using FileRepositories;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserRepository _repository;
    public AuthController(IUserRepository repository)
    {
        this._repository = repository;
    }

    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> RegisterUser([FromBody] RegisterRequest request)
    {
        if (request.Password != request.ConfirmPassword)
        {
            return BadRequest("Passwords do not match");
        }
        var user = new User()
        {
            Username = request.Username,
            Password = request.Password,
        };
        User created = await _repository.AddAsync(user);
        var dto = new UserDto(created.Id, created.Username);
        return Created($"/auth/{dto.Id}", dto);
    }
    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> LoginUser([FromBody] LoginRequest request)
    {
        var users = ( _repository.GetManyAsync()).ToList();

        var user = users.FirstOrDefault(u => u.Username == request.Username && u.Password == request.Password);

        if (user == null)
            return Unauthorized("Invalid username or password");

        return Ok(new UserDto(user.Id, user.Username));
    }

}