using Microsoft.AspNetCore.Mvc;
using RentalApp.Core.DTOs.Requests;
using RentalApp.Core.Services.IServices;

namespace RentalApp.Api.Controllers
{
    [Route("entregadores")]
    [ApiController]
    public class RenterController : ControllerBase
    {
        private readonly IRenterService _renterRepository;
        public RenterController(IRenterService renterRepository)
        {
            _renterRepository = renterRepository;
        }
        [HttpPost]
        public void Post([FromBody] RegisterRenterRequest value)
        {
            //var result = _renterRepository.
        }


        [HttpPost("{Id}/cnh")]
        public void PostDriverLicenseImg([FromBody] string value)
        {
        }
    }
}
