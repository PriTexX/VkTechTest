using Microsoft.AspNetCore.Mvc;
using VkTechTest.Contracts.GetUserEndpoint;
using VkTechTest.Mappers;
using VkTechTest.Repositories.Interfaces;

namespace VkTechTest.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserRepository _userRepository;

    public UserController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    [HttpGet("{login}")]
    public async Task<IActionResult> GetUserAsync(GetUserRequest request)
    {
        var user = await _userRepository.GetUserWithStateAndGroupByLoginAsync(request.Login);

        if (user is null)
        {
            return NotFound();
        }

        return Ok(UserMapper.MapFromDBUser(user));
    }

    [HttpGet]
    public async Task<IActionResult> GetAllUsersAsync()
    {
        var users = _userRepository.GetAllUsersWithStateAndGroupAsync();
        return Ok(UserMapper.MapFromDBUsers(users));
    }
}