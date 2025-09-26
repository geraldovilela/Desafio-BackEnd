using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalApp.Core.DTOs.Requests
{
    public class MotorcycleCreatedEventRequest
    {
        public int Id { get; set; }
        public string Identifier { get; set; } = string.Empty;
        public int Year { get; set; }
        public string Model { get; set; } = string.Empty;
        public string VehiclePlate { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
