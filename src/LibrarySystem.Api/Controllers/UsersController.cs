using LibrarySystem.Api.DTOs.Users;
using LibrarySystem.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    public async Task<ActionResult<UserResponse>> Create(
        CreateUserRequest request)
    {
        var user = await _userService.CreateAsync(request);

        return Created($"/api/users/{user.Id}", user);
    }

    [HttpGet]
    public async Task<ActionResult<List<UserResponse>>> GetAll()
    {
        var users = await _userService.GetAllAsync();

        return Ok(users);
    }
}