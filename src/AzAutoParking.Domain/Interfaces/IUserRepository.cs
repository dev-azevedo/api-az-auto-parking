using AzAutoParking.Domain.Models;

namespace AzAutoParking.Domain.Interfaces;

public interface IUserRepository : IGenericRepository<User>
{
    Task<User?> GetByEmailAsync(string email);
}