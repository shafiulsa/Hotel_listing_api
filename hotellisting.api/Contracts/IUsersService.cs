using hotellisting.api.DTOs.Auth;
using hotellisting.api.Results;

namespace hotellisting.api.Contracts;

public interface IUsersService
{
    Task<Result<RegisteredUserDto>> RegisterAsync(RegisterUserDto registerUserDto);
    Task<Result<string>> LoginAsync(LoginUserDto dto);
}
