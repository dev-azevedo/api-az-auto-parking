using AzAutoParking.Domain.Models;

namespace AzAutoParking.Domain.Interfaces;

public interface IGenericRepository<T> where T : BaseModel
{
    Task<(List<T>, int)> GetAllAsync(int skip, int take);
    Task<T?> GetByIdAsync(long id);
    Task<T> CreateAsync(T item);
    Task<T> UpdateAsync(T item);
}