using hotellisting.api.Constants;
using hotellisting.api.Contracts;
using hotellisting.api.data;
using hotellisting.api.DTOs.Auth;
using hotellisting.api.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace hotellisting.api.Controllers;

[ApiController]
[Route("api/[controller]")]
[AllowAnonymous]
public class AuthController(IUsersService usersService) : BaseApiController
{
[HttpPost("register")]
public async Task<ActionResult<RegisteredUserDto>> Register(RegisterUserDto registerUserDto)
{
    var result = await usersService.RegisterAsync(registerUserDto);
    return ToActionResult(result);
}

[HttpPost("login")]
public async Task<ActionResult<string>> Login(LoginUserDto loginUserDto)
{
    var result = await usersService.LoginAsync(loginUserDto);
    return ToActionResult(result);
}
}
