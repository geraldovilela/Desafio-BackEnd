using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalApp.Messaging.Config
{
    public class RabbitMQConfig
    {
        public string HostName { get; set; } = "localhost";
        public int Port { get; set; } = 5672;
        public string UserName { get; set; } = "guest";
        public string Password { get; set; } = "guest";
        public string VirtualHost { get; set; } = "/";
        public int ConnectionTimeout { get; set; } = 5000;
        public int HeartbeatInterval { get; set; } = 10000;
    }
}
