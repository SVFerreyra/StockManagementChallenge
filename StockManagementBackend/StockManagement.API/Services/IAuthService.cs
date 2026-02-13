using StockManagement.API.Models.DTOs;

namespace StockManagement.API.Services
{
    public interface IAuthService
    {
        Task<LoginResponse?> AuthenticateAsync(LoginRequest request);
        string GenerateJwtToken(string username);
        string HashPassword(string password);
        bool VerifyPassword(string password, string passwordHash);
    }
}