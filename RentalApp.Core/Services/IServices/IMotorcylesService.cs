using RentalApp.Core.DTOs.Requests;
using RentalApp.Core.DTOs.Responses;

namespace RentalApp.Core.Services.IServices
{
    public interface IMotorcyclesService
    {
        Task<List<MotorcycleResponse>> GetMotorcycles();
        Task<MotorcycleResponse?> GetMotorcycleById(string id);
        Task<ServiceResponse<MotorcycleResponse>> CreateMotorcycle(CreateMotorcycleRequest request);
        Task<ServiceResponse<MotorcycleResponse>> UpdatePlate(string id, UpdatePlateRequest request);
        Task<ServiceResponse<bool>> DeleteMotorcycle(string id);
    }
}
