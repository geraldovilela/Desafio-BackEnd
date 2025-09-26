
namespace RentalApp.Messaging.Models
{
    public interface IMotorcycle2024Notification
    {
        int Id { get; }
        string Identifier { get; }
        string VehiclePlate { get; }
        string Model { get; }
        string Message { get; }
        DateTime Timestamp { get; }
    }
}
