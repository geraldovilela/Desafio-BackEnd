using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalApp.Messaging.Models
{
    public class RabbitMQHealthInfo
    {
        public string Status { get; set; } = "Unknown";
        public bool IsHealthy { get; set; } = false;
        public string Host { get; set; } = "";
        public int Port { get; set; }
        public string VirtualHost { get; set; } = "";
        public string Username { get; set; } = "";
        public DateTime? LastChecked { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
