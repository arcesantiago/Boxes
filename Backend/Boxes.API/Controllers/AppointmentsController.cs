using Boxes.Application.DTOs;
using Boxes.Application.Features.Appointments.Commands.CreateAppointment;
using Boxes.Application.Features.Appointments.Queries.GetAllAppointments;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Boxes.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AppointmentsController : ControllerBase
{
    private readonly IMediator _mediator;

    public AppointmentsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<int>> CreateAppointment(
        [FromBody] CreateAppointmentDto dto,
        CancellationToken cancellationToken)
    {
        var command = new CreateAppointmentCommand(
            dto.PlaceId,
            dto.AppointmentAt,
            dto.ServiceType,
            dto.Contact,
            dto.Vehicle);

        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetAllAppointments), new { id = result }, result);
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<AppointmentDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<AppointmentDto>>> GetAllAppointments(
        CancellationToken cancellationToken)
    {
        return Ok(await _mediator.Send(new GetAllAppointmentsQuery(), cancellationToken));

    }
}