using AzAutoParking.Domain.Models;

namespace AzAutoParking.Tests.Mock;

public static class UserMock
{
    public static User CreateUser()
    {
        return new User
        {
            FullName = "Jhonatan Azevedo",
            Email = "jhonatan.azevedo@gmail.com",
            Password = "Teste@123",
            ConfirmedAccount = true,
            IsAdmin = true
        };
    }
}