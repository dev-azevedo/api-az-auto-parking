using System.Data;
using AzAutoParking.Domain.Interfaces;
using AzAutoParking.Domain.Models;
using AzAutoParking.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace AzAutoParking.Infra.Data.Repository;

public class GenericRepository<T> : IGenericRepository<T> where T : BaseModel
{
    protected readonly AppDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public GenericRepository(AppDbContext context, DbSet<T> dbSet)
    {
        _context = context;
        _dbSet = dbSet;
    }

    public async Task<(List<T>, int)> GetAllAsync(int skip = 1, int take = 10)
    {
        var totalItems = await _dbSet.CountAsync();
        var setSkip = (skip - 1) + take;
        var items = await _dbSet.Skip(setSkip).Take(take).Where(c => c.IsActive).ToListAsync();
        
        return (items, totalItems);
    }

    public async Task<T?> GetByIdAsync(long id)
    {
        return await _dbSet.FirstOrDefaultAsync(c => c.Id == id && c.IsActive);
    }

    public async Task<T> CreateAsync(T item)
    {
        await _dbSet.AddAsync(item);
        await _context.SaveChangesAsync();
        return item;
    }

    public async Task<T> UpdateAsync(T item)
    {
        _context.Entry(item).CurrentValues.SetValues(item);
        await _context.SaveChangesAsync();
        return item;
    }
}