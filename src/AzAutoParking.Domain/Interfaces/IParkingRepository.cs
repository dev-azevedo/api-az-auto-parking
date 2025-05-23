﻿using AzAutoParking.Domain.Models;

namespace AzAutoParking.Domain.Interfaces;

public interface IParkingRepository : IGenericRepository<Parking>
{
    Task<Parking?> GetByParkingNumberAsync(int parkingNumber);
}