﻿namespace AzAutoParking.Infra.ExternalServices.Interfaces;

public interface IPasswordHasherService
{
    string HashPassword(string password);
    bool VerifyHashedPassword(string password, string hashedPassword);
}