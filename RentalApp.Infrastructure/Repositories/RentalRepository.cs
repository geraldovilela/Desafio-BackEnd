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
    public class RentalRepository(RentalDBContext context) : GenericRepository<Rental>(context), IRentalRepository
    {
        public async Task<IEnumerable<Rental>> GetRentalsByRenterAsync(string renterId)
        {
            return await FindWithIncludesAsync(
                r => r.RenterId == renterId,
                r => r.Motorcycle,
                r => r.Renter
            );
        }

        public async Task<IEnumerable<Rental>> GetRentalsByMotorcycleAsync(string motorcycleId)
        {
            return await FindWithIncludesAsync(
                r => r.MotorcycleId == motorcycleId,
                r => r.Motorcycle,
                r => r.Renter
            );
        }

        public async Task<Rental?> GetActiveRentalByMotorcycleAsync(string motorcycleId)
        {
            return await FirstOrDefaultAsync(r => r.MotorcycleId == motorcycleId && r.ActualEndDate == null);
        }

        public async Task<Rental?> GetActiveRentalByRenterAsync(string renterId)
        {
            return await FirstOrDefaultAsync(r => r.RenterId == renterId && r.ActualEndDate == null);
        }

        public async Task<IEnumerable<Rental>> GetRentalsWithDetailsAsync()
        {
            return await GetAllWithIncludesAsync(
                r => r.Motorcycle,
                r => r.Renter
            );
        }

        public async Task<decimal> CalculateRentalCostAsync(Guid rentalId, DateTime returnDate)
        {
            var rental = await GetByIdAsync(rentalId);
            if (rental == null) return 0;

            var rentalDays = (returnDate - rental.StartDate).Days;
            var plannedDays = rental.PlanDays;
            var dailyPrice = rental.DailyPrice;

            decimal totalCost = plannedDays * dailyPrice;

            if (rentalDays < plannedDays)
            {
                // Devolução antecipada - aplicar multa
                var unusedDays = plannedDays - rentalDays;
                var penaltyRate = rental.PlanDays switch
                {
                    7 => 0.20m,
                    15 => 0.40m,
                    _ => 0m
                };

                var penalty = unusedDays * dailyPrice * penaltyRate;
                totalCost = (rentalDays * dailyPrice) + penalty;
            }
            else if (rentalDays > plannedDays)
            {
                // Devolução tardia - cobrar dias extras
                var extraDays = rentalDays - plannedDays;
                totalCost += extraDays * 50.00m; // R$ 50,00 por dia adicional
            }

            return totalCost;
        }

        public async Task<bool> HasActiveRentalAsync(string renterId)
        {
            return await AnyAsync(r => r.RenterId == renterId && r.ActualEndDate == null);
        }
    }

}
