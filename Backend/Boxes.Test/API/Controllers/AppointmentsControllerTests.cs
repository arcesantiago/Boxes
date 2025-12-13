using Boxes.API.Controllers;
using Boxes.Application.DTOs;
using Boxes.Application.Features.Appointments.Commands.CreateAppointment;
using Boxes.Application.Features.Appointments.Queries.GetAllAppointments;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Boxes.Test.API.Controllers;

public class AppointmentsControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly AppointmentsController _controller;

    public AppointmentsControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new AppointmentsController(_mediatorMock.Object);
    }

    [Fact]
    public async Task CreateAppointment_WithValidDto_ShouldReturnCreated()
    {
        // Arrange
        var dto = new CreateAppointmentDto(
            PlaceId: 1,
            AppointmentAt: DateTime.UtcNow.AddDays(1),
            ServiceType: "Cambio de aceite",
            Contact: new ContactDto("Juan Pérez", "juan@example.com", "+5491123456789"),
            Vehicle: new VehicleDto("Toyota", "Corolla", 2020, "ABC123")
        );

        var appointmentId = 1;

        _mediatorMock
            .Setup(x => x.Send(It.IsAny<CreateAppointmentCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(appointmentId);

        // Act
        var result = await _controller.CreateAppointment(dto, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        var createdAtActionResult = result.Result.Should().BeOfType<CreatedAtActionResult>().Subject;
        createdAtActionResult.Value.Should().Be(appointmentId);
        createdAtActionResult.StatusCode.Should().Be(201);
    }

    [Fact]
    public async Task GetAllAppointments_WithAppointments_ShouldReturnOk()
    {
        // Arrange
        var appointments = new List<AppointmentDto>
        {
            new AppointmentDto(1, 1, DateTime.UtcNow.AddDays(1), "Servicio 1",
                new ContactDto("Test 1", "test1@example.com", null), null, DateTimeOffset.UtcNow),
            new AppointmentDto(2, 1, DateTime.UtcNow.AddDays(2), "Servicio 2",
                new ContactDto("Test 2", "test2@example.com", null), null, DateTimeOffset.UtcNow)
        };

        _mediatorMock
            .Setup(x => x.Send(It.IsAny<GetAllAppointmentsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(appointments);

        // Act
        var result = await _controller.GetAllAppointments(CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.Value.Should().BeEquivalentTo(appointments);
    }

    [Fact]
    public async Task GetAllAppointments_WithNoAppointments_ShouldReturnEmptyList()
    {
        // Arrange
        var emptyList = new List<AppointmentDto>();

        _mediatorMock
            .Setup(x => x.Send(It.IsAny<GetAllAppointmentsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(emptyList);

        // Act
        var result = await _controller.GetAllAppointments(CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        var value = okResult.Value.Should().BeAssignableTo<IEnumerable<AppointmentDto>>().Subject;
        value.Should().BeEmpty();
    }
}

