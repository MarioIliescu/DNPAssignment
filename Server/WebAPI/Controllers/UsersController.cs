using ApiContracts.DTOs;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserRepository _userRepository;

    public UsersController(IUserRepository userRepository)
    {
        this._userRepository = userRepository;
    }

    [HttpPost]
    public async Task<ActionResult<UserDto>> AddUser([FromBody] CreateUserDto request)
    {
        User user = new()
        {
            Username = request.Username,
            Password = request.Password,
        };
        User created = await _userRepository.AddAsync(user);

        UserDto dto = new(created.Id, created.Username);
        return Created($"/users/{dto.Id}", dto);
    }

    [HttpPut]
    public async Task<ActionResult> UpdateUser([FromBody] UpdateUserDto request)
    {
        User? verifyUser = await _userRepository.GetSingleAsync(request.Id);
        if (verifyUser.Password.Equals(request.Password))
        {
            User user = new()
            {
                Id = verifyUser.Id,
                Username = verifyUser.Username,
                Password = request.NewPassword,
            };
            try
            {
                await _userRepository.UpdateAsync(user);
                return NoContent();
            }
            catch
            {
                return StatusCode(500, "Internal server error");
            }
        }

        return StatusCode(500, "Password is incorrect");
    }

    [HttpDelete("{userId}")]
    public async Task<ActionResult> DeleteUserAsync([FromBody] DeleteUserDto request)
    {
        try
        {
            await _userRepository.DeleteAsync(request.Id);
            return NoContent();
        }
        catch
        {
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet]
    public async Task<ActionResult> GetUsersAsync([FromQuery] string? nameContains)
    {
        IQueryable<User> users = _userRepository.GetManyAsync();
        List<UserDto> userDtos = users.Select(u => new UserDto(u.Id, u.Username)).ToList();
        return Ok(userDtos);
    }

    [HttpGet("{userId}")]
    public async Task<ActionResult<UserDto>> GetSingleUser(int userId)
    {
        User user = await _userRepository.GetSingleAsync(userId);

        var dto = new UserDto(user.Id, user.Username);
        return Ok(dto);
    }
}