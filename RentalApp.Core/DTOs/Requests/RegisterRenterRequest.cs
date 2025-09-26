namespace RentalApp.Core.DTOs.Requests
{
    public class RegisterRenterRequest
    {
        public string Identificador { get; set; }
        public string Nome { get; set; }
        public string Cnpj { get; set; }
        public DateTime Data_nascimento { get; set; }
        public string NumeroCnh { get; set; }
        public string TipoCnh { get; set; }
        public string ImagemCnh { get; set; }
    }
}
