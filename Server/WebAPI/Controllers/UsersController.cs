using ApiContracts.DTOs;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserRepository userRepository;

    public UsersController(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    [HttpPost]
    public async Task<ActionResult<UserDto>> AddUser([FromBody] CreateUserDto request)
    {
        User user = new()
        {
            Username = request.Username,
            Password = request.Password,
        };
        User created = await userRepository.AddAsync(user);

        UserDto dto = new(created.Id, created.Username);
        return Created($"/users/{dto.Id}", dto);
    }

    [HttpPut]
    public async Task<ActionResult> UpdateUser([FromBody] UpdateUserDto request)
    {
        User? verifyUser = await userRepository.GetSingleAsync(request.Id);
        if (verifyUser == null)
        {
            return StatusCode(404, "User not found");
        }

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
                await userRepository.UpdateAsync(user);
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
            User? user = await userRepository.GetSingleAsync(request.Id);
            if (user == null)
                return StatusCode(404, "User not found");

            await userRepository.DeleteAsync(user.Id); // now user.Id is correct
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
        IQueryable<User> users = userRepository.GetManyAsync();
        List<UserDto> userDtos = users.Select(u => new UserDto(u.Id, u.Username)).ToList();
        return Ok(userDtos);
    }

    [HttpGet("{userId}")]
    public async Task<ActionResult<UserDto>> GetSingleUser(int userId)
    {
        User user = await userRepository.GetSingleAsync(userId);
        if (user == null)
        {
            return StatusCode(404, "User not found");
        }

        var dto = new UserDto(user.Id, user.Username);
        return Ok(dto);
    }
}