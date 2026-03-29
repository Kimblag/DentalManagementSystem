using DentalSystem.Application.DTOs.Specialties.Inputs;
using DentalSystem.Application.DTOs.Specialties.Outputs;
using DentalSystem.Application.UseCases.Specialties;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DentalSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpecialtyController : ControllerBase
    {
        private readonly CreateSpecialtyUseCase _createUseCase;
        private readonly GetSpecialtyByIdUseCase _getByIdUseCase;

        public SpecialtyController(CreateSpecialtyUseCase createUseCase,
            GetSpecialtyByIdUseCase getByIdUseCase
            )
        {
            _createUseCase = createUseCase;
            _getByIdUseCase = getByIdUseCase;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SpecialtyResponse>> GetById(Guid id)
        {
            var response = await _getByIdUseCase.HandleAsync(id);
            return Ok(response);
        }


        [HttpPost(Name = "CreateSpecialty")]
        public async Task<ActionResult<SpecialtyResponse>> Create(CreateSpecialtyRequest request)
        {
            var response = await _createUseCase.HandleAsync(request);

            return CreatedAtAction(
                nameof(GetById),
                new { id = response.Id },
                response);
        }
    }
}
