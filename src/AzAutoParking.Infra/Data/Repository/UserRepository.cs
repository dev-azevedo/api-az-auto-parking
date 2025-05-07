using AzAutoParking.Domain.Interfaces;
using AzAutoParking.Domain.Models;
using AzAutoParking.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace AzAutoParking.Infra.Data.Repository;

public class UserRepository(AppDbContext context, DbSet<User> dbSet) : GenericRepository<User>(context, dbSet), IUserRepository;