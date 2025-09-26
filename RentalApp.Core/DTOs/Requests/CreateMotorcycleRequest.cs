using System.ComponentModel.DataAnnotations;

namespace RentalApp.Core.DTOs.Requests
{
    public class CreateMotorcycleRequest
    {
        [Required(ErrorMessage = "Identificador é obrigatório")]
        [StringLength(50, ErrorMessage = "Identificador deve ter no máximo 50 caracteres")]
        public string Identificador { get; set; } = string.Empty;

        [Required(ErrorMessage = "Ano é obrigatório")]
        [Range(1900, 2030, ErrorMessage = "Ano deve estar entre 1900 e 2030")]
        public int Ano { get; set; }

        [Required(ErrorMessage = "Modelo é obrigatório")]
        [StringLength(100, ErrorMessage = "Modelo deve ter no máximo 100 caracteres")]
        public string Modelo { get; set; } = string.Empty;

        [Required(ErrorMessage = "Placa é obrigatória")]
        [RegularExpression(@"^[A-Z]{3}[0-9]{4}$|^[A-Z]{3}[0-9][A-Z][0-9]{2}$",
            ErrorMessage = "Placa deve estar no formato ABC1234 ou ABC1D23")]
        public string Placa { get; set; } = string.Empty;
    }
}
