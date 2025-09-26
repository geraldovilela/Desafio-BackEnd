using RentalApp.Core.Entities;
using RentalApp.Infrastructure.Repositories.Interfaces.RentalApp.Core.Interfaces;

namespace RentalApp.Infrastructure.Repositories.Interfaces
{
    public interface IMotorcycleRepository : IRepository<Motorcycle>
    {
        Task<Motorcycle?> GetByPlateAsync(string plate);
        Task<Motorcycle?> GetByIdentifierAsync(string identifier);
        Task<bool> IsPlateUniqueAsync(string plate, int? excludeId = null);
        Task<IEnumerable<Motorcycle>> GetMotorcyclesWithoutRentalsAsync();
        Task<bool> HasActiveRentalsAsync(int motorcycleId);
    }
}
