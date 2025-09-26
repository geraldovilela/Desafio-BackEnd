using System.ComponentModel.DataAnnotations;

namespace RentalApp.Core.Entities
{
    public class Rental
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string MotorcycleId { get; set; }
        public Motorcycle Motorcycle { get; set; }
        [Required]
        public string RenterId { get; set; }
        public Renter Renter { get; set; }

        public int PlanDays { get; set; }
        public decimal DailyPrice { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime ExpectedEndDate { get; set; }
        public DateTime? ActualEndDate { get; set; }
        public decimal? TotalAmount { get; set; }

        public DateTime CreatedAt { get; set; }
    }

}
