using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalApp.Messaging.Config
{
    public static class RabbitMQQueues
    {
        public const string MotorcycleRegistered = "motorcycle.registered";
        public const string MotorcycleYear2024Notification = "motorcycle.year2024.notification";

        // Exchange names
        public const string MotorcycleExchange = "motorcycle.exchange";

        // Routing keys
        public const string MotorcycleRegisteredRoutingKey = "motorcycle.registered";
    }
}
