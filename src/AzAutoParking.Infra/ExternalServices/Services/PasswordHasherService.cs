using BCrypt.Net;
using AzAutoParking.Infra.ExternalServices.Interfaces;

namespace AzAutoParking.Infra.ExternalServices.Services;

public class PasswordHasherService : IPasswordHasherService
{   
    public string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public bool VerifyHashedPassword(string password, string hashedPassword)
    {
        return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
    }
}