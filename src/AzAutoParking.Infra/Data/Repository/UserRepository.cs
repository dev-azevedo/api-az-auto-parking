using AzAutoParking.Domain.Interfaces;
using AzAutoParking.Domain.Models;
using AzAutoParking.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace AzAutoParking.Infra.Data.Repository;

public class UserRepository(AppDbContext context) : GenericRepository<User>(context), IUserRepository
{
    public async Task<User?> GetByEmailAsync(string email)
    {
        return await DbSet.FirstOrDefaultAsync(u => u.Email == email);
    }
}