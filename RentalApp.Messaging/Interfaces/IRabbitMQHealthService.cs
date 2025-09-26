using RentalApp.Messaging.Models;

namespace RentalApp.Messaging.Interface
{
    public interface IRabbitMQHealthService
    {
        Task<bool> IsHealthyAsync();
        Task<RabbitMQHealthInfo> GetHealthInfoAsync();
    }
}
