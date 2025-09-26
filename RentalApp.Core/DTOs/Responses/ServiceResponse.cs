using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalApp.Core.DTOs.Responses
{
    public class ServiceResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public List<string> Errors { get; set; } = new();

    }

    public static class ServiceResponseExtensions
    {
        public static ServiceResponse<T> SuccessResponse<T>(T data, string message = "Operação realizada com sucesso.")
        {
            return new ServiceResponse<T>
            {
                Success = true,
                Message = message,
                Data = data
            };
        }

        public static ServiceResponse<T> ErrorResponse<T>(string message, List<string>? errors = null)
        {
            return new ServiceResponse<T>
            {
                Success = false,
                Message = message,
                Data = default(T),
                Errors = errors ?? new List<string>()
            };
        }
    }
}
