using RentalApp.Core.Entities;

namespace RentalApp.Messaging.Interface
{
    public interface IMessagePublisher
    {
        Task PublishAsync<T>(string queueName, T message) where T : class;
        Task PublishMotorcycleRegisteredAsync(Motorcycle motorcycle);
    }
}
