using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RentalApp.Messaging.Config;
using RentalApp.Messaging.Interface;
using RentalApp.Messaging.Models;

namespace RentalApp.Messaging.Health
{
    public class RabbitMQHealthService : IRabbitMQHealthService
    {
        private readonly RabbitMQConfig _config;
        private readonly ILogger<RabbitMQHealthService> _logger;

        public RabbitMQHealthService(IOptions<RabbitMQConfig> config, ILogger<RabbitMQHealthService> logger)
        {
            _config = config.Value;
            _logger = logger;
        }

        public async Task<bool> IsHealthyAsync()
        {
            try
            {
                return await TestConnectionAsync();
            }
            catch (Exception ex)
            {
                _logger.LogWarning("RabbitMQ health check failed: {Error}", ex.Message);
                return false;
            }
        }

        public async Task<RabbitMQHealthInfo> GetHealthInfoAsync()
        {
            var healthInfo = new RabbitMQHealthInfo
            {
                Host = _config.HostName,
                Port = _config.Port,
                VirtualHost = _config.VirtualHost,
                Username = _config.UserName
            };

            try
            {
                var isConnected = await TestConnectionAsync();
                healthInfo.Status = isConnected ? "Connected" : "Disconnected";
                healthInfo.IsHealthy = isConnected;

                if (isConnected)
                {
                    healthInfo.LastChecked = DateTime.UtcNow;
                    _logger.LogDebug("RabbitMQ health check successful");
                }
                else
                {
                    _logger.LogWarning("RabbitMQ health check failed - connection test returned false");
                }
            }
            catch (Exception ex)
            {
                healthInfo.Status = "Error";
                healthInfo.IsHealthy = false;
                healthInfo.ErrorMessage = ex.Message;
                _logger.LogError(ex, "Error checking RabbitMQ health");
            }

            return healthInfo;
        }

        private async Task<bool> TestConnectionAsync()
        {
            try
            {
                var factory = new ConnectionFactory
                {
                    HostName = _config.HostName,
                    Port = _config.Port,
                    UserName = _config.UserName,
                    Password = _config.Password,
                    VirtualHost = _config.VirtualHost,
                    RequestedConnectionTimeout = TimeSpan.FromMilliseconds(_config.ConnectionTimeout),
                    RequestedHeartbeat = TimeSpan.FromMilliseconds(_config.HeartbeatInterval)
                };

                
                using var connection = await factory.CreateConnectionAsync();
                using var channel = await connection.CreateChannelAsync();

                var isConnected = connection.IsOpen && channel.IsOpen;

                if (isConnected)
                {
                    _logger.LogDebug("RabbitMQ connection test successful");
                }

                return isConnected;
            }
            catch (RabbitMQ.Client.Exceptions.BrokerUnreachableException ex)
            {
                _logger.LogWarning("RabbitMQ broker unreachable: {Error}", ex.Message);
                return false;
            }
            catch (RabbitMQ.Client.Exceptions.AuthenticationFailureException ex)
            {
                _logger.LogError("RabbitMQ authentication failed: {Error}", ex.Message);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogWarning("RabbitMQ connection test failed: {Error}", ex.Message);
                return false;
            }
        }
    }
}