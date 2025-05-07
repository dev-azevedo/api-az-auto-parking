using AzAutoParking.Domain.Interfaces;
using AzAutoParking.Domain.Models;
using AzAutoParking.Infra.Data.Context;

namespace AzAutoParking.Infra.Data.Repository;

public class UserRepository(AppDbContext context) : GenericRepository<User>(context), IUserRepository;