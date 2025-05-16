using System.Security.Claims;
using AzAutoParking.Domain.Models;

namespace AzAutoParking.Application.Interfaces;

public interface IJwtService
{
    string GenerateJwtToken(long id, string email, string fullname, bool isAdmin, bool resetPassword = false, int expirationMinutes = 1440);
    ClaimsPrincipal? ValidateToken(string token);
}