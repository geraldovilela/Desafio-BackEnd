using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RentalApp.Infrastructure.DBContext;
using RentalApp.Infrastructure.Entities;
using RentalApp.Messaging.Models;

namespace RentalApp.Messaging.Consumer
{
    public class Motorcycle2024Consumer : IConsumer<IMotorcycle2024Notification>
    {
        private readonly ILogger<Motorcycle2024Consumer> _logger;
        private readonly IServiceProvider _serviceProvider;

        public Motorcycle2024Consumer(
            ILogger<Motorcycle2024Consumer> logger,
            IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public async Task Consume(ConsumeContext<IMotorcycle2024Notification> context)
        {
            var message = context.Message;

            try
            {
                _logger.LogInformation("Processing motorcycle 2024 notification for motorcycle {Id}: {Message}",
                    message.Id, message.Message);

                using var scope = _serviceProvider.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<RentalDBContext>();

                var notification = new Motorcycle2024Notification
                {
                    Id = Guid.NewGuid(),
                    MotorcycleId = message.Id,
                    Message = message.Message,
                    CreatedAt = message.Timestamp,
                    ProcessedAt = DateTime.UtcNow
                };

                await dbContext.Motorcycle2024Notifications.AddAsync(notification);
                await dbContext.SaveChangesAsync();

                _logger.LogInformation("Motorcycle 2024 notification processed and saved successfully. Motorcycle ID: {MotorcycleId}",
                    message.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing motorcycle 2024 notification for motorcycle {Id}",
                    message.Id);
                
                throw;
            }
        }
    }
}
