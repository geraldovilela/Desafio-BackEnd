using RentalApp.Core.Entities;
using RentalApp.Infrastructure.Repositories.Interfaces.RentalApp.Core.Interfaces;

namespace RentalApp.Infrastructure.Repositories.Interfaces
{
    public interface IRenterRepository : IRepository<Renter>
    {
        Task<Renter?> GetByCompanyRegistrationAsync(string companyRegistration);
        Task<Renter?> GetByDriverLicenseAsync(string driverLicense);
        Task<bool> IsCompanyRegistrationUniqueAsync(string companyRegistration, string? excludeId = null);
        Task<bool> IsDriverLicenseUniqueAsync(string driverLicense, string? excludeId = null);
        Task<IEnumerable<Renter>> GetRentersByLicenseTypeAsync(string licenseType);
    }
}
