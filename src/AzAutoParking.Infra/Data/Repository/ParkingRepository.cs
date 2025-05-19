using AzAutoParking.Domain.Interfaces;
using AzAutoParking.Domain.Models;
using AzAutoParking.Infra.Data.Context;

namespace AzAutoParking.Infra.Data.Repository;

public class ParkingRepository(AppDbContext context) : GenericRepository<Parking>(context), IParkingRepository
{ }