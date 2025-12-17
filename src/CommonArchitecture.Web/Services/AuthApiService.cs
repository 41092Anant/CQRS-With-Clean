using CommonArchitecture.Application.DTOs;
using System.Text;
using System.Text.Json;

namespace CommonArchitecture.Web.Services;

public class AuthApiService : IAuthApiService
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonOptions;

    public AuthApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }

    public async Task<SendOtpResponseDto> SendOtpAsync(SendOtpRequestDto request)
    {
        try
        {
            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            var response = await _httpClient.PostAsync("/api/auth/send-otp", content);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return JsonSerializer.Deserialize<SendOtpResponseDto>(responseContent, _jsonOptions) 
                    ?? new SendOtpResponseDto { Success = false, Message = "Failed to parse response" };
            }

            var errorResponse = JsonSerializer.Deserialize<SendOtpResponseDto>(responseContent, _jsonOptions);
            return errorResponse ?? new SendOtpResponseDto { Success = false, Message = "Failed to send OTP" };
        }
        catch (Exception ex)
        {
            return new SendOtpResponseDto 
            { 
                Success = false, 
                Message = $"Error: {ex.Message}" 
            };
        }
    }

    public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request)
    {
        try
        {
            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            var response = await _httpClient.PostAsync("/api/auth/login", content);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return JsonSerializer.Deserialize<LoginResponseDto>(responseContent, _jsonOptions) 
                    ?? new LoginResponseDto { Success = false, Message = "Failed to parse response" };
            }

            var errorResponse = JsonSerializer.Deserialize<LoginResponseDto>(responseContent, _jsonOptions);
            return errorResponse ?? new LoginResponseDto { Success = false, Message = "Login failed" };
        }
        catch (Exception ex)
        {
            return new LoginResponseDto 
            { 
                Success = false, 
                Message = $"Error: {ex.Message}" 
            };
        }
    }

    public async Task<bool> LogoutAsync()
    {
        try
        {
            var response = await _httpClient.PostAsync("/api/auth/logout", null);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }
}
