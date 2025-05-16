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
        var query = DbSet.AsNoTracking().Where(m => m.IsActive);
        
        var totalItems = await query.CountAsync();
        var setSkip = (skip - 1) * take;
        var items = await query.Skip(setSkip).Take(take).ToListAsync();
        
        return (items, totalItems);
    }

    public async Task<T?> GetByIdAsync(long id)
    {
        return await DbSet.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id && c.IsActive);
    }

    public async Task<T> CreateAsync(T item)
    {
        await DbSet.AddAsync(item);
        await Context.SaveChangesAsync();
        return item;
    }

    public async Task<T> UpdateAsync(T item)
    {
        DbSet.Update(item);
        await Context.SaveChangesAsync();
        return item;
    }
}