using AzAutoParking.Domain.Models;

namespace AzAutoParking.Application.Interfaces;

public interface IJwtService
{
    string GenerateJwtToken(string email, string fullname, long id);
}