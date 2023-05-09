using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using VkTechTest.Contracts;
using VkTechTest.Contracts.Common;
using VkTechTest.Mappers;
using VkTechTest.Models.Enums;
using VkTechTest.Models.Exceptions;
using VkTechTest.Repositories.Interfaces;
using VkTechTest.Services.Interfaces;

namespace VkTechTest.Controllers;

/// <summary>
/// Взаимодействие с пользователями
/// </summary>
[ApiController]
[Authorize]
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

    /// <summary>
    /// Возвращает пользователя по указанному логину
    /// </summary>
    /// <remarks>Пользователь не может получить информацию о другом пользователе(за исключением админа)</remarks>
    /// <param name="login">Логин, указанный при регистрации</param>
    /// <returns>Информацию о пользователе</returns>
    /// <response code="200">Успешное завершение</response>
    /// <response code="403">Попытка доступа к другому пользователю</response>
    /// <response code="404">Не найден пользователь с указанным логином</response>
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
    [HttpGet("{login:alpha}")]
    public async Task<IActionResult> GetUserAsync(string login)
    {
        if (NotAdminIsTryingToAccessAnotherUser(login))
        {
            return new BadRequestObjectResult(new ErrorResponse("Cannot access other users information")){StatusCode = 403};
        }
        
        var user = await _userRepository.GetUserWithStateAndGroupByLoginAsync(login);

        if (user is null)
        {
            return NotFound();
        }

        return Ok(UserMapper.MapFromDBUser(user));
    }

    /// <summary>
    /// Возвращает список пользователей
    /// </summary>
    /// <remarks>Только админ может получать список</remarks>
    /// <returns>Список пользователей</returns>
    /// <response code="200">Успешное завершение</response>
    /// <response code="400"></response>
    /// <response code="403">Ошибка доступа</response>
    [ProducesResponseType(typeof(List<UserResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAllUsersAsync([FromQuery]GetAllUsersRequest request)
    {
        if (request.PageSize > _options.CurrentValue.MaxPageSize)
        {
            return new BadRequestObjectResult(new ErrorResponse($"Cannot set page size more than {_options.CurrentValue.MaxPageSize}"));
        }
        
        var users = _userRepository.GetAllUsersWithStateAndGroupAsync(request.PageSize, request.OffSet); 
        return Ok(UserMapper.MapFromDBUsers(users)); // Здесь не будет блокировки, даже без вызова await foreach
    }

    /// <summary>
    /// Создает нового пользователя
    /// </summary>
    /// <returns>Созданный пользователь</returns>
    /// <response code="201">Успешное завершение</response>
    /// <response code="409">Пользователь с таким логином уже существует</response>
    [ProducesResponseType(typeof(CreateUserResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> CreateUserAsync([FromBody]CreateUserRequest request)
    {
        try
        {
            var user = await _userService.RegisterAsync(
                request.Login,
                request.Password);

            return new OkObjectResult(new CreateUserResponse
            {
                Id = user.Id,
                CreatedDate = user.CreatedDate,
                Login = user.Login,
                UserGroup = UserGroupType.User,
                UserState = UserStateType.Active
            }){StatusCode = StatusCodes.Status201Created};
        }
        catch (UserAlreadyExistsException err)
        {
            return new ConflictObjectResult(new { error = err.Message });
        }
        catch (UserRegistrationDelayException err)
        {
            return new BadRequestObjectResult(new { error = err.Message });
        }
    }

    /// <summary>
    /// Удаляет аккаунт пользователя
    /// </summary>
    /// <remarks>Пользователь может удалить только свой аккаунт (исключение - админ)</remarks>
    /// <param name="login">Логин</param>
    /// <returns>Логин удаленного пользователя</returns>
    /// <response code="200">Успешное завершение</response>
    /// <response code="404">Пользователь не найден</response>
    /// <response code="403">Попытка удалить другого пользователя</response>
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
    [HttpDelete]
    [Route("{login:alpha}")]
    public async Task<IActionResult> DeleteUserAsync(string login)
    {
        if (NotAdminIsTryingToAccessAnotherUser(login))
        {
            return new BadRequestObjectResult(new ErrorResponse("Cannot delete another user"));
        }
        
        try
        {
            await _userService.RemoveUserAsync(login);
            return new OkObjectResult(new { deletedUserLogin = login });
        }
        catch (UserNotFoundException err)
        {
            return new BadRequestObjectResult(new {error = err.Message});
        }
    }

    private bool NotAdminIsTryingToAccessAnotherUser(string accessedUserLogin)
    {
        var userLogin = User.Claims.First(c => c.Type == "login").Value;
        var isAdmin = User.Claims.Any(c => c.Type == ClaimTypes.Role && c.Value == "Admin");

        return !isAdmin && userLogin != accessedUserLogin;
    }
}