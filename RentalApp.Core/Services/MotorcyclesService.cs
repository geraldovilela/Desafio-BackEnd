using AutoMapper;
using Microsoft.Extensions.Logging;
using RentalApp.Core.DTOs.Requests;
using RentalApp.Core.DTOs.Responses;
using RentalApp.Core.Entities;
using RentalApp.Core.Services.IServices;
using RentalApp.Infrastructure.Repositories.Interfaces;
using RentalApp.Messaging.Interface;

namespace RentalApp.Core.Services
{
    public class MotorcyclesService : IMotorcyclesService
    {
        private readonly IMotorcycleRepository _motorcycleRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<MotorcyclesService> _logger;
        private readonly IMessagePublisher _messagePublisher;

        public MotorcyclesService(
            IMotorcycleRepository motorcycleRepository,
            IMapper mapper,
            ILogger<MotorcyclesService> logger,
            IMessagePublisher messagePublisher)
        {
            _motorcycleRepository = motorcycleRepository;
            _mapper = mapper;
            _logger = logger;
            _messagePublisher = messagePublisher;
        }

        public async Task<List<MotorcycleResponse>> GetMotorcycles()
        {
            try
            {
                _logger.LogInformation("Querying motorcycles.");

                IEnumerable<Motorcycle> motorcycles;
                
                motorcycles = await _motorcycleRepository.GetAllAsync();
                
                var result = _mapper.Map<List<MotorcycleResponse>>(motorcycles);

                _logger.LogInformation("Query successful. Total motorcycles: {Count}", result.Count);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error querying motorcycles");
                return new List<MotorcycleResponse>();
            }
        }

        public async Task<MotorcycleResponse?> GetMotorcycleById(string id)
        {
            try
            {
                _logger.LogInformation("Querying motorcycle by ID: {Id}", id);

                if (!int.TryParse(id, out int motorcycleId))
                {
                    _logger.LogWarning("Invalid ID provided: {Id}", id);
                    return null;
                }

                var motorcycle = await _motorcycleRepository.GetByIdAsync(motorcycleId);

                if (motorcycle == null)
                {
                    _logger.LogWarning("Motorcycle not found with ID: {Id}", id);
                    return null;
                }

                var result = _mapper.Map<MotorcycleResponse>(motorcycle);

                _logger.LogInformation("Motorcycle successfully found: {Id}", id);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error querying motorcycle by ID: {Id}", id);
                return null;
            }
        }

        public async Task<ServiceResponse<MotorcycleResponse>> CreateMotorcycle(CreateMotorcycleRequest request)
        {
            try
            {
                var motorcycle = _mapper.Map<CreateMotorcycleRequest, Motorcycle>(request);
                _logger.LogInformation("Starting creation of a new motorcycle. Plate: {Plate}", motorcycle.VehiclePlate);

                if (!await _motorcycleRepository.IsPlateUniqueAsync(motorcycle.VehiclePlate))
                {
                    _logger.LogWarning("Attempt to register motorcycle with an existing plate: {Plate}", motorcycle.VehiclePlate);
                    return ServiceResponseExtensions.ErrorResponse<MotorcycleResponse>("Plate already exists in the system");
                }

                var createdMotorcycle = await _motorcycleRepository.AddAsync(motorcycle);

                _logger.LogInformation("Motorcycle successfully created. ID: {Id}, Plate: {Plate}",
                    createdMotorcycle.Id, createdMotorcycle.VehiclePlate);

                await PublishMotorcycleCreatedEvent(createdMotorcycle);

                var response = _mapper.Map<MotorcycleResponse>(createdMotorcycle);

                return ServiceResponseExtensions.SuccessResponse(response, "Motorcycle successfully registered");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating motorcycle");
                return ServiceResponseExtensions.ErrorResponse<MotorcycleResponse>("Internal error while registering motorcycle");
            }
        }

        public async Task<ServiceResponse<MotorcycleResponse>> UpdatePlate(string id, UpdatePlateRequest request)
        {
            try
            {
                _logger.LogInformation("Starting Vehicle Plate update. ID: {Id}, New Plate: {Plate}", id, request.Placa);

                if (string.IsNullOrEmpty(id))
                {
                    _logger.LogWarning("Invalid ID provided for update: {Id}", id);
                    return ServiceResponseExtensions.ErrorResponse<MotorcycleResponse>("Invalid ID");
                }

                var existingMotorcycle = await _motorcycleRepository.GetByIdentifierAsync(id);
                if (existingMotorcycle == null)
                {
                    _logger.LogWarning("Motorcycle not found for update. ID: {Id}", id);
                    return ServiceResponseExtensions.ErrorResponse<MotorcycleResponse>("Motorcycle not found");
                }

                if (!await _motorcycleRepository.IsPlateUniqueAsync(request.Placa, existingMotorcycle.Id))
                {
                    _logger.LogWarning("Attempting to update to an existing plate: {Plate}", request.Placa);
                    return ServiceResponseExtensions.ErrorResponse<MotorcycleResponse>("New plate already exists in the system");
                }

                existingMotorcycle.VehiclePlate = request.Placa.ToUpperInvariant();
                existingMotorcycle.UpdatedAt = DateTime.UtcNow;

                var updatedMotorcycle = await _motorcycleRepository.UpdateAsync(existingMotorcycle);

                _logger.LogInformation("Plate updated successfully. ID: {Id}, New Plate: {Plate}",
                    existingMotorcycle.Id, updatedMotorcycle.VehiclePlate);

                var response = _mapper.Map<MotorcycleResponse>(updatedMotorcycle);

                return ServiceResponseExtensions.SuccessResponse(response, "Vehicle Plate updated successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating motorcycle license plate. ID: {Id}", id);
                return ServiceResponseExtensions.ErrorResponse<MotorcycleResponse>("Internal error while updating vehicle plate");
            }
        }

        public async Task<ServiceResponse<bool>> DeleteMotorcycle(string id)
        {
            try
            {
                _logger.LogInformation("Starting motorcycle removal. ID: {Id}", id);

                if (string.IsNullOrWhiteSpace(id))
                {
                    _logger.LogWarning("Invalid ID provided for removal: {Id}", id);
                    return ServiceResponseExtensions.ErrorResponse<bool>("Invalid ID");
                }

                var existingMotorcycle = await _motorcycleRepository.GetByIdentifierAsync(id);
                if (existingMotorcycle == null)
                {
                    _logger.LogWarning("Motorcycle not found for removal. ID: {Id}", id);
                    return ServiceResponseExtensions.ErrorResponse<bool>("Motorcycle not found");
                }

                if (await _motorcycleRepository.HasActiveRentalsAsync(existingMotorcycle.Id))
                {
                    _logger.LogWarning("Attempt to remove motorcycle with active rentals. ID: {Id}", id);
                    return ServiceResponseExtensions.ErrorResponse<bool>("It is not possible to remove a motorcycle with an active rental");
                }

                await _motorcycleRepository.DeleteAsync(existingMotorcycle.Id);

                _logger.LogInformation("Motorcycle successfully removed. ID: {Id}", id);

                return ServiceResponseExtensions.SuccessResponse<bool>(true, "Motorcycle successfully removed");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing motorcycle. ID: {Id}", id);
                return ServiceResponseExtensions.ErrorResponse<bool>("Internal error while removing motorcycle");
            }
        }

        private async Task PublishMotorcycleCreatedEvent(Motorcycle motorcycle)
        {
            try
            {
                var motorcycleCreatedEvent = new MotorcycleCreatedEventRequest
                {
                    Id = motorcycle.Id,
                    Identifier = motorcycle.Identifier,
                    Year = motorcycle.Year,
                    Model = motorcycle.Model,
                    VehiclePlate = motorcycle.VehiclePlate,
                    CreatedAt = motorcycle.CreatedAt
                };

                await _messagePublisher.PublishMotorcycleRegisteredAsync(motorcycle);

                _logger.LogInformation("Motorcycle created event published successfully. ID: {Id}", motorcycle.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error publishing created motorcycle event. ID: {Id}", motorcycle.Id);
            }
        }
    }
}
