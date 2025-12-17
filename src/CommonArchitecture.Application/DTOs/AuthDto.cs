namespace CommonArchitecture.Application.DTOs;

public class LoginRequestDto
{
    public string Mobile { get; set; } = string.Empty;
    public string Otp { get; set; } = string.Empty;
}

public class LoginResponseDto
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public UserDto? User { get; set; }
}

public class SendOtpRequestDto
{
    public string Mobile { get; set; } = string.Empty;
}

public class SendOtpResponseDto
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public string? Otp { get; set; } // Only for development/testing
}
