using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalApp.Infrastructure.Entities
{
    public class Motorcycle2024Notification
    {
        public Guid Id { get; set; }
        public int MotorcycleId { get; set; }
        public string Message { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime ProcessedAt { get; set; }
    }
}
