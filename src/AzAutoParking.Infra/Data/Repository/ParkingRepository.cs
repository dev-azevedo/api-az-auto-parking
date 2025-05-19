using AzAutoParking.Domain.Interfaces;
using AzAutoParking.Domain.Models;
using AzAutoParking.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace AzAutoParking.Infra.Data.Repository;

public class ParkingRepository(AppDbContext context) : GenericRepository<Parking>(context), IParkingRepository
{
    public async Task<Parking?> GetByParkingNumberAsync(int parkingNumber)
    {
        return await DbSet.AsNoTracking().FirstOrDefaultAsync(p => p.ParkingNumber == parkingNumber);
    }
}