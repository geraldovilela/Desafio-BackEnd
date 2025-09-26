using System.ComponentModel.DataAnnotations;

namespace RentalApp.Core.DTOs.Requests
{
    public class UpdatePlateRequest
    {
        [Required(ErrorMessage = "Placa é obrigatória")]
        [RegularExpression(@"^[A-Z]{3}[0-9]{4}$|^[A-Z]{3}[0-9][A-Z][0-9]{2}$",
            ErrorMessage = "Placa deve estar no formato ABC1234 ou ABC1D23")]
        public string Placa { get; set; } = string.Empty;
    }
}
