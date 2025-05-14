using System.Security.Claims;
using AzAutoParking.Domain.Models;

namespace AzAutoParking.Application.Interfaces;

public interface IJwtService
{
    string GenerateJwtToken(long id, string email, string fullname, bool isAdmin);
    ClaimsPrincipal? ValidateToken(string token);
}