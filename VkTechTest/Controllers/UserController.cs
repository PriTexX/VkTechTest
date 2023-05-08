﻿using Microsoft.AspNetCore.Mvc;
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

    public UserController(IUserRepository userRepository, IUserService userService)
    {
        _userRepository = userRepository;
        _userService = userService;
    }

    [HttpGet("{login}")]
    public async Task<IActionResult> GetUserAsync([FromRoute]GetUserRequest request)
    {
        var user = await _userRepository.GetUserWithStateAndGroupByLoginAsync(request.Login);

        if (user is null)
        {
            return NotFound();
        }

        return Ok(UserMapper.MapFromDBUser(user));
    }

    [HttpGet]
    public async Task<IActionResult> GetAllUsersAsync(GetAllUsersRequest request)
    {
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
    [Route("{login}")]
    public async Task<IActionResult> DeleteUserAsync([FromRoute]DeleteUserRequest request)
    {
        try
        {
            await _userRepository.DeleteUserByLoginAsync(request.Login);
            return new OkObjectResult(new { deletedUserLogin = request.Login });
        }
        catch (UserNotFoundException err)
        {
            return new BadRequestObjectResult(new {error = err.Message});
        }
    }
}