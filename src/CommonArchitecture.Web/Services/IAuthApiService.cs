using CommonArchitecture.Application.DTOs;

namespace CommonArchitecture.Web.Services;

public interface IAuthApiService
{
    Task<SendOtpResponseDto> SendOtpAsync(SendOtpRequestDto request);
    Task<LoginResponseDto> LoginAsync(LoginRequestDto request);
    Task<bool> LogoutAsync();
}
