using CommonArchitecture.Application.DTOs;
using CommonArchitecture.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CommonArchitecture.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthRepository _authRepository;
    private const string FIXED_OTP = "1234";

    public AuthController(IAuthRepository authRepository)
    {
        _authRepository = authRepository;
    }

    [HttpPost("send-otp")]
    public async Task<ActionResult<SendOtpResponseDto>> SendOtp([FromBody] SendOtpRequestDto request)
    {
        if (string.IsNullOrWhiteSpace(request.Mobile))
        {
            return BadRequest(new SendOtpResponseDto
            {
                Success = false,
                Message = "Mobile number is required"
            });
        }

        // Validate mobile number format (10 digits)
        if (request.Mobile.Length != 10 || !request.Mobile.All(char.IsDigit))
        {
            return BadRequest(new SendOtpResponseDto
            {
                Success = false,
                Message = "Please enter a valid 10-digit mobile number"
            });
        }

        // Check if user exists with this mobile number
        var user = await _authRepository.GetUserByMobileAsync(request.Mobile);
        if (user == null)
        {
            return NotFound(new SendOtpResponseDto
            {
                Success = false,
                Message = "User not found with this mobile number"
            });
        }

        // In production, send OTP via SMS service
        // For now, return fixed OTP in development
        return Ok(new SendOtpResponseDto
        {
            Success = true,
            Message = "OTP sent successfully",
            Otp = FIXED_OTP // Remove this in production
        });
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginRequestDto request)
    {
        if (string.IsNullOrWhiteSpace(request.Mobile) || string.IsNullOrWhiteSpace(request.Otp))
        {
            return BadRequest(new LoginResponseDto
            {
                Success = false,
                Message = "Mobile number and OTP are required"
            });
        }

        // Validate OTP
        if (request.Otp != FIXED_OTP)
        {
            return Unauthorized(new LoginResponseDto
            {
                Success = false,
                Message = "Invalid OTP"
            });
        }

        // Get user by mobile
        var user = await _authRepository.GetUserByMobileAsync(request.Mobile);
        if (user == null)
        {
            return NotFound(new LoginResponseDto
            {
                Success = false,
                Message = "User not found"
            });
        }

        // Map to UserDto
        var userDto = new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Mobile = user.Mobile,
            RoleId = user.RoleId,
            RoleName = user.Role?.RoleName ?? string.Empty,
            ProfileImagePath = user.ProfileImagePath
        };

        return Ok(new LoginResponseDto
        {
            Success = true,
            Message = "Login successful",
            User = userDto
        });
    }

    [HttpPost("logout")]
    public IActionResult Logout()
    {
        return Ok(new { success = true, message = "Logout successful" });
    }
}
