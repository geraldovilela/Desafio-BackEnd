using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RentalApp.Api.Controllers
{
    [Route("locacao")]
    [ApiController]
    public class RentalController : ControllerBase
    {
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        [HttpGet("{Id}")]
        public void Get()
        {
        }

        [HttpPut("{Id}/devolucao")]
        public void Put([FromBody] string value)
        {
        }


    }
}
