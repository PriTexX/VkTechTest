using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using VkTechTest.Contracts;
using VkTechTest.Mappers;
using VkTechTest.Models.Enums;
using VkTechTest.Models.Exceptions;
using VkTechTest.Repositories.Interfaces;
using VkTechTest.Services.Interfaces;

namespace VkTechTest.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly IUserService _userService;
    private readonly IOptionsMonitor<ApplicationOptions> _options;

    public UserController(IUserRepository userRepository, IUserService userService, IOptionsMonitor<ApplicationOptions> options)
    {
        _userRepository = userRepository;
        _userService = userService;
        _options = options;
    }

    [HttpGet("{login:alpha}")]
    public async Task<IActionResult> GetUserAsync(string login)
    {
        var user = await _userRepository.GetUserWithStateAndGroupByLoginAsync(login);

        if (user is null)
        {
            return NotFound();
        }

        return Ok(UserMapper.MapFromDBUser(user));
    }

    [HttpGet]
    public async Task<IActionResult> GetAllUsersAsync([FromQuery]GetAllUsersRequest request)
    {
        if (request.PageSize > _options.CurrentValue.MaxPageSize)
        {
            return new BadRequestObjectResult(new { error = $"Cannot set page size more than {_options.CurrentValue.MaxPageSize}" });
        }
        
        var users = _userRepository.GetAllUsersWithStateAndGroupAsync(request.PageSize, request.OffSet); 
        return Ok(UserMapper.MapFromDBUsers(users)); // Здесь не будет блокировки, даже без вызова await foreach
    }

    [HttpPost]
    public async Task<IActionResult> CreateUserAsync(CreateUserRequest request)
    {
        try
        {
            var user = await _userService.RegisterAsync(
                request.Login,
                request.Password);

            return Ok(new CreateUserResponse
            {
                Id = user.Id,
                CreatedDate = user.CreatedDate,
                Login = user.Login,
                UserGroup = UserGroupType.User,
                UserState = UserStateType.Active
            });
        }
        catch (UserAlreadyExistsException err)
        {
            return new BadRequestObjectResult(new { error = err.Message });
        }
        catch (UserRegistrationDelayException err)
        {
            return new BadRequestObjectResult(new { error = err.Message });
        }
    }

    [HttpDelete]
    [Route("{login:alpha}")]
    public async Task<IActionResult> DeleteUserAsync(string login)
    {
        try
        {
            await _userRepository.DeleteUserByLoginAsync(login);
            return new OkObjectResult(new { deletedUserLogin = login });
        }
        catch (UserNotFoundException err)
        {
            return new BadRequestObjectResult(new {error = err.Message});
        }
    }
}