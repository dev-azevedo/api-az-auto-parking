using AzAutoParking.Application.Response;
using AzAutoParking.Tests.Mock;

namespace AzAutoParking.Tests.Domain.Models;

public class UserTests
{
    [Fact]
    public void ShouldCreateUser()
    {
        var user = UserMock.CreateUser();
        
        Assert.NotNull(user);
        Assert.Equal("Jhonatan Azevedo", user.FullName);
        Assert.Equal("jhonatan.azevedo@gmail.com", user.Email);
        Assert.Equal("Teste@123", user.Password);
        Assert.True(user.ConfirmedAccount);
        Assert.True(user.IsAdmin);
    }
}