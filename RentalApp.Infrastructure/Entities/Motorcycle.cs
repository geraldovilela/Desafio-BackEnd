using System.ComponentModel.DataAnnotations;

namespace RentalApp.Core.Entities
{
    public class Motorcycle
    {
        [Key]
        public int Id   { get; set; }
        [Required]
        public string Identifier { get; set; }
        [Required]
        public int Year { get; set; }
        [Required]
        public string Model { get; set; }
        [Required]
        public string VehiclePlate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

}
