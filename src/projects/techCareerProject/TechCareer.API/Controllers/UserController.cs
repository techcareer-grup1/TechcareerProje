using Microsoft.AspNetCore.Mvc;
using TechCareer.Models.Dtos.Users.Request;
using TechCareer.Service.Abstracts;
using Core.Security.Entities;
using Core.Security.Hashing;

namespace TechCareer.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController(IUserService userService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllUsers() => Ok(await userService.GetListAsync());

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetUserById([FromRoute] int id)
    {
        var user = await userService.GetAsync(u => u.Id == id);
        if (user is null)
            return NotFound("User not found");
        return Ok(user);
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
    {
        HashingHelper.CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

        User user = new User
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            PasswordHash = passwordHash, 
            PasswordSalt = passwordSalt, 
            Status = request.Status
        };

        var createdUser = await userService.AddAsync(user);
        return Ok(createdUser);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateUser([FromBody] UpdateUserRequest request)
    {
        var existingUser = await userService.GetAsync(u => u.Id == request.Id);
        if (existingUser is null)
            return NotFound("User not found");

        existingUser.FirstName = request.FirstName;
        existingUser.LastName = request.LastName;
        existingUser.Email = request.Email;
        existingUser.Status = request.Status;

        var updatedUser = await userService.UpdateAsync(existingUser);
        return Ok(updatedUser);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteUser([FromRoute] int id)
    {
        var existingUser = await userService.GetAsync(u => u.Id == id);
        if (existingUser is null)
            return NotFound("User not found");

        var deletedUser = await userService.DeleteAsync(existingUser, false);
        return Ok(deletedUser);
    }
}
