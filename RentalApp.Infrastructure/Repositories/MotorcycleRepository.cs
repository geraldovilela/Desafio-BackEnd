using Microsoft.EntityFrameworkCore;
using RentalApp.Core.Entities;
using RentalApp.Infrastructure.DBContext;
using RentalApp.Infrastructure.Repositories.Interfaces;
using RentalApp.Infrastructure.Repositories.Interfaces.RentalApp.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalApp.Infrastructure.Repositories
{
    public class MotorcycleRepository(RentalDBContext context) : GenericRepository<Motorcycle>(context), IMotorcycleRepository
    {
        public async Task<Motorcycle?> GetByPlateAsync(string plate)
        {
            return await FirstOrDefaultAsync(m => m.VehiclePlate == plate);
        }

        public async Task<Motorcycle?> GetByIdentifierAsync(string identifier)
        {
            return await FirstOrDefaultAsync(m => m.Identifier == identifier);
        }

        public async Task<bool> IsPlateUniqueAsync(string plate, int? excludeId = null)
        {
            if (excludeId.HasValue)
            {
                return !await AnyAsync(m => m.VehiclePlate == plate && m.Id != excludeId.Value);
            }
            return !await AnyAsync(m => m.VehiclePlate == plate);
        }

        public async Task<IEnumerable<Motorcycle>> GetMotorcyclesWithoutRentalsAsync()
        {
            return await _dbSet.AsNoTracking()
                .Where(m => !_context.Set<Rental>().Any(r => r.MotorcycleId == m.Id.ToString() && r.ActualEndDate == null))
                .ToListAsync();
        }

        public async Task<bool> HasActiveRentalsAsync(int motorcycleId)
        {
            return await _context.Set<Rental>()
                .AnyAsync(r => r.MotorcycleId == motorcycleId.ToString() && r.ActualEndDate == null);
        }
    }
}
