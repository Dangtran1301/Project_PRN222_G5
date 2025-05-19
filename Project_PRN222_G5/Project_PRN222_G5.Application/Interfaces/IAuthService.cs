using Project_PRN222_G5.Application.DTOs.Requests;
using Project_PRN222_G5.Application.DTOs.Users.Responses;

namespace Project_PRN222_G5.Application.Interfaces;

public interface IAuthService
{
    Task<LoginResponse> LoginAsync(LoginRequest loginRequest);
}