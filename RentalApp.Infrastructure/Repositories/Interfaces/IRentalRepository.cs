using RentalApp.Core.Entities;
using RentalApp.Infrastructure.Repositories.Interfaces.RentalApp.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalApp.Infrastructure.Repositories.Interfaces
{
    public interface IRentalRepository : IRepository<Rental>
    {
        Task<IEnumerable<Rental>> GetRentalsByRenterAsync(string renterId);
        Task<IEnumerable<Rental>> GetRentalsByMotorcycleAsync(string motorcycleId);
        Task<Rental?> GetActiveRentalByMotorcycleAsync(string motorcycleId);
        Task<Rental?> GetActiveRentalByRenterAsync(string renterId);
        Task<IEnumerable<Rental>> GetRentalsWithDetailsAsync();
        Task<decimal> CalculateRentalCostAsync(Guid rentalId, DateTime returnDate);
        Task<bool> HasActiveRentalAsync(string renterId);
    }
}
