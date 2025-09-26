using MassTransit;
using Microsoft.Extensions.Logging;
using RentalApp.Core.Entities;
using RentalApp.Messaging.Interface;
using RentalApp.Messaging.Models;

namespace RentalApp.Messaging.Publisher
{
    public class RabbitMqPublisher(IBus bus, ILogger<RabbitMqPublisher> logger) : IMessagePublisher
    {
        public async Task PublishAsync<T>(string queueName, T message) where T : class
        {
            try
            {
                logger.LogInformation("Publishing message to queue {QueueName}", queueName);
                
                await bus.Publish(message);

                logger.LogInformation("Message published successfully to queue {QueueName}", queueName);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error publishing message to queue {QueueName}", queueName);
                throw;
            }
        }

        public async Task PublishMotorcycleRegisteredAsync(Motorcycle motorcycleEvent)
        {
            try
            {
                logger.LogInformation("Publishing motorcycle registered event for motorcycle {Id} - Year: {Year}",
                    motorcycleEvent.Id, motorcycleEvent.Year);
                
                if (motorcycleEvent.Year == 2024)
                {
                    logger.LogInformation("Motorcycle {Id} is from 2024, publishing specific notification",
                        motorcycleEvent.Id);

                    await bus.Publish<IMotorcycle2024Notification>(new
                    {
                        motorcycleEvent.Id,
                        motorcycleEvent.Identifier,
                        motorcycleEvent.VehiclePlate,
                        motorcycleEvent.Model,
                        Message = $"New Motorcycle Registered {motorcycleEvent.Model} - {motorcycleEvent.VehiclePlate}",
                        Timestamp = DateTime.UtcNow
                    });
                }

                logger.LogInformation("Motorcycle registered event published successfully for motorcycle {Id}",
                    motorcycleEvent.Id);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error publishing motorcycle registered event for motorcycle {Id}",
                    motorcycleEvent.Id);
                throw;
            }
        }
    }
}