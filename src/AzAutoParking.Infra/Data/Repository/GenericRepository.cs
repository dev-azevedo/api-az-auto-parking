using AzAutoParking.Domain.Interfaces;
using AzAutoParking.Domain.Models;
using AzAutoParking.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace AzAutoParking.Infra.Data.Repository;

public class GenericRepository<T> : IGenericRepository<T> where T : BaseModel
{
    protected readonly AppDbContext Context;
    protected readonly DbSet<T> DbSet;

    protected GenericRepository(AppDbContext context)
    {
        Context = context;
        DbSet = Context.Set<T>();
    }

    public async Task<(List<T>, int)> GetAllAsync(int skip = 1, int take = 10)
    {
        var totalItems = await DbSet.CountAsync();
        var setSkip = (skip - 1) * take;
        var items = await DbSet.Skip(setSkip).Take(take).Where(c => c.IsActive).ToListAsync();
        
        return (items, totalItems);
    }

    public async Task<T?> GetByIdAsync(long id)
    {
        return await DbSet.FirstOrDefaultAsync(c => c.Id == id && c.IsActive);
    }

    public async Task<T> CreateAsync(T item)
    {
        await DbSet.AddAsync(item);
        await Context.SaveChangesAsync();
        return item;
    }

    public async Task<T> UpdateAsync(T item)
    {
        Context.Entry(item).CurrentValues.SetValues(item);
        await Context.SaveChangesAsync();
        return item;
    }
}