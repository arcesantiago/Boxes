using Boxes.Application.DTOs;
using Boxes.Application.Features.Appointments.Queries.GetAllWorkshops;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Boxes.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WorkshopsController : ControllerBase
{
    private readonly IMediator _mediator;

    public WorkshopsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<WorkshopDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<WorkshopDto>>> GetAllWorkshops(
        CancellationToken cancellationToken)
    {
        return Ok(await _mediator.Send(new GetAllWorkshopsQuery(), cancellationToken));
    }
}