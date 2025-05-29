// using AzAutoParking.Domain.Interfaces;
// using AzAutoParking.Domain.Models;
// using AzAutoParking.Infra.Data.Context;
// using AzAutoParking.Infra.Data.Repository;
// using AzAutoParking.Tests.Infra.Data.Context;
// using System.Linq;
// using System.Linq;
// using System.Threading.Tasks;
// using Xunit;
//
// namespace AzAutoParking.Tests.Infra.Data.Repositories;
//
// public class TypeAutomobileRepositoryTests
// {
//     private static readonly AppDbContext _context = new AppDbContextInMemory().CreateDbContextInMemory();
//     private readonly ITypeAutomobileRepository _repo = new TypeAutomobileRepository(_context);
//
//     [Fact]
//     public async Task CreateTypeAutomobileSuccess()
//     {
//         var nameTypeAutomobile = "Test Automobile";
//         var typeAutomobile = new TypeAutomobile()
//         {
//             Name = nameTypeAutomobile
//         };
//         
//         await _repo.CreateAsync(typeAutomobile);
//         var result = await _repo.GetByIdAsync(typeAutomobile.Id);
//         
//         Assert.NotNull(result);
//         Assert.Equal(nameTypeAutomobile, result.Name);
//     }
//
//     [Fact]
//     public async Task GetAllTypeAutomobileSuccess()
//     {
//         var (items, totalItems) = await _repo.GetAllAsync(1, 10);
//         
//         Assert.NotNull(items); Assert.Equal(1, totalItems);
//     }
// }
