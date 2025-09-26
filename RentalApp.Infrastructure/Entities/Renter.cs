using System.ComponentModel.DataAnnotations;

namespace RentalApp.Core.Entities
{
    public class Renter
    {
        [Key]
        public string RenterId { get; set; }
        public string Name { get; set; }
        [Required]
        public string CompanyRegistrationNumber { get; set; }
        public DateTime Birthdate { get; set; }
        [Required]
        public string DriverLicenseNumber { get; set; }
        [Required]
        public string DriverLicenseType { get; set; }
        public string DriverLicenseImgString { get; set; }
        public DateTime CreatedAt { get; set; }
    }

}
