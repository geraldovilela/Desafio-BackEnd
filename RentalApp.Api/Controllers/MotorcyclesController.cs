using Microsoft.AspNetCore.Mvc;
using RentalApp.Core.DTOs.Requests;
using RentalApp.Core.DTOs.Responses;
using RentalApp.Core.Services.IServices;

namespace RentalApp.Api.Controllers
{
    [ApiController]
    [Route("motos")]
    [Produces("application/json")]
    public class MotorcyclesController(IMotorcyclesService motorcyclesService) : ControllerBase
    {
        private readonly IMotorcyclesService _motorcyclesService = motorcyclesService;

        /// <summary>
        /// Gets all motorcycles
        /// </summary>
        /// <returns>A list of motorcycles.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<MotorcycleResponse>), 200)]
        public async Task<IActionResult> GetMotorcycles()
        {
            var result = await _motorcyclesService.GetMotorcycles();
            return Ok(result);
        }

        /// <summary>
        /// Gets a motorcycle by its ID.
        /// </summary>
        /// <param name="id">The motorcycle ID.</param>
        /// <returns>The motorcycle data.</returns>

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(MotorcycleResponse), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        public async Task<IActionResult> GetMotorcycleById([FromRoute] string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest(new { mensagem = "Request mal formada" });
            }

            var result = await _motorcyclesService.GetMotorcycleById(id);

            if (result == null)
            {
                return NotFound(new { mensagem = "Moto não encontrada" });
            }

            return Ok(result);
        }

        /// <summary>
        /// Registers a new motorcycle.
        /// </summary>
        /// <param name="request">The motorcycle data to be registered.</param>
        /// <returns>The creation status.</returns>

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        public async Task<IActionResult> CreateMotorcycle([FromBody] CreateMotorcycleRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { mensagem = "Dados inválidos" });
            }

            var result = await _motorcyclesService.CreateMotorcycle(request);

            if (!result.Success)
            {
                return BadRequest(new { mensagem = "Dados inválidos" });
            }

            return Created();
        }

        /// <summary>
        /// Updates the license plate of a motorcycle.
        /// </summary>
        /// <param name="id">The motorcycle ID.</param>
        /// <param name="request">The new license plate.</param>
        /// <returns>A success message.</returns>

        [HttpPut("{id}/placa")]
        [ProducesResponseType(typeof(SuccessResponse), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        public async Task<IActionResult> UpdateMotorcyclePlate(
            [FromRoute] string id,
            [FromBody] UpdatePlateRequest request)
        {
            if (!ModelState.IsValid || string.IsNullOrWhiteSpace(id))
            {
                return BadRequest(new { mensagem = "Dados inválidos" });
            }

            var result = await _motorcyclesService.UpdatePlate(id, request);

            if (!result.Success)
            {
                return BadRequest(new { mensagem = "Dados inválidos" });
            }

            return Ok(new { mensagem = "Placa modificada com sucesso" });
        }

        /// <summary>
        /// Removes a motorcycle.
        /// </summary>
        /// <param name="id">The motorcycle ID.</param>
        /// <returns>A confirmation of removal.</returns>

        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        public async Task<IActionResult> DeleteMotorcycle([FromRoute] string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest(new { mensagem = "Dados inválidos" });
            }

            var result = await _motorcyclesService.DeleteMotorcycle(id);

            if (!result.Success)
            {
                return BadRequest(new { mensagem = "Dados inválidos" });
            }

            return Ok();
        }
    }
}