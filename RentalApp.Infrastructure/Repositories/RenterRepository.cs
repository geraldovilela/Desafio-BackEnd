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
    public class RenterRepository(RentalDBContext context) : GenericRepository<Renter>(context), IRenterRepository
    {
        public async Task<Renter?> GetByCompanyRegistrationAsync(string companyRegistration)
        {
            return await FirstOrDefaultAsync(r => r.CompanyRegistrationNumber == companyRegistration);
        }

        public async Task<Renter?> GetByDriverLicenseAsync(string driverLicense)
        {
            return await FirstOrDefaultAsync(r => r.DriverLicenseNumber == driverLicense);
        }

        public async Task<bool> IsCompanyRegistrationUniqueAsync(string companyRegistration, string? excludeId = null)
        {
            if (!string.IsNullOrEmpty(excludeId))
            {
                return !await AnyAsync(r => r.CompanyRegistrationNumber == companyRegistration && r.RenterId != excludeId);
            }
            return !await AnyAsync(r => r.CompanyRegistrationNumber == companyRegistration);
        }

        public async Task<bool> IsDriverLicenseUniqueAsync(string driverLicense, string? excludeId = null)
        {
            if (!string.IsNullOrEmpty(excludeId))
            {
                return !await AnyAsync(r => r.DriverLicenseNumber == driverLicense && r.RenterId != excludeId);
            }
            return !await AnyAsync(r => r.DriverLicenseNumber == driverLicense);
        }

        public async Task<IEnumerable<Renter>> GetRentersByLicenseTypeAsync(string licenseType)
        {
            return await FindAsync(r => r.DriverLicenseType == licenseType || r.DriverLicenseType == "A+B");
        }
    }
}
