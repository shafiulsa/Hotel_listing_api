using hotellisting.api.Constants;
using hotellisting.api.Constants;
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
public class AuthController(UserManager<ApplicationUser> userManager) : BaseApiController
{
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterUserDto registerUserDto)
    {
        var user = new ApplicationUser
        {
            Email = registerUserDto.Email,
            FirstName = registerUserDto.FirstName,
            LastName = registerUserDto.LastName,
            UserName = registerUserDto.Email
        };

        var result = await userManager.CreateAsync(user, registerUserDto.Password);
        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => new Error(ErrorCodes.BadRequest, e.Description)).ToArray();
            return MapErrorsToResponse(errors);
        }

        // Optional: Send confirmation Email
        return Ok();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginUserDto loginUserDto)
    {
        var user = await userManager.FindByEmailAsync(loginUserDto.Email);
        if (user == null)
        {
            return Unauthorized(new { message = "Invalid Credentials" });
        }

        var isPasswordValid = await userManager.CheckPasswordAsync(user, loginUserDto.Password);
        if (!isPasswordValid)
        {
            return Unauthorized(new { message = "Invalid Credentials" });
        }

        return Ok();
    }
}
