using DentalSystem.Api.Contracts.Specialties;
using DentalSystem.Application.UseCases.Specialties.CreateSpecialty;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DentalSystem.Application.Contracts.Specialties;

namespace DentalSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public sealed class SpecialtiesController : ControllerBase
    {
   
        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateSpecialtyRequest request,
            [FromServices] CreateSpecialtyHandler handler,
            CancellationToken cancellationToken)
        {
            // Create the command
            CreateSpecialtyCommand command = new(
                request.Name,
                request.Treatments.Select(t => 
                    new TreatmentInput(t.Name, t.BaseCost, t.Description)).ToList(),
                request.Description);

            // Execute handler
            await handler.Handle(command, cancellationToken);


            return Created();
        }
    }
}
