using System.Security.Claims;

namespace AzAutoParking.Infra.ExternalServices.Interfaces;

public interface IJwtService
{
    string GenerateJwtToken(long id, string email, string fullname, bool isAdmin, bool resetPassword = false, int expirationMinutes = 1440);
    ClaimsPrincipal? ValidateToken(string token);
}