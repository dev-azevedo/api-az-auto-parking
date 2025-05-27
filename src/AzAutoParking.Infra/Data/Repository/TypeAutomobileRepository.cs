using AzAutoParking.Domain.Interfaces;
using AzAutoParking.Domain.Models;
using AzAutoParking.Infra.Data.Context;

namespace AzAutoParking.Infra.Data.Repository;

public class TypeAutomobileRepository(AppDbContext context) : GenericRepository<TypeAutomobile>(context), ITypeAutomobileRepository
{}