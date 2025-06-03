using AzAutoParking.Domain.Interfaces;
using AzAutoParking.Domain.Models;
using AzAutoParking.Infra.Data.Context;

namespace AzAutoParking.Infra.Data.Repository;

public class AutomobileRepository(AppDbContext context) : GenericRepository<Automobile>(context), IAutomobileRepository
{}