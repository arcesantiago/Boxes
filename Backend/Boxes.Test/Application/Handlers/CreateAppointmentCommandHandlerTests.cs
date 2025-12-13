using AutoMapper;
using Boxes.Application.Contracts.Interfaces;
using Boxes.Application.Contracts.Persistence;
using Boxes.Application.DTOs;
using Boxes.Application.Features.Appointments.Commands.CreateAppointment;
using Boxes.Domain.Common;
using Boxes.Domain.Entities;
using FluentAssertions;
using Moq;

namespace Boxes.Test.Application.Handlers;

public class CreateAppointmentCommandHandlerTests
{
    private readonly Mock<IAppointmentRepository> _repositoryMock;
    private readonly Mock<IWorkshopService> _workshopServiceMock;
    private readonly Mock<IAppointmentUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly CreateAppointmentCommandHandler _handler;

    public CreateAppointmentCommandHandlerTests()
    {
        _repositoryMock = new Mock<IAppointmentRepository>();
        _workshopServiceMock = new Mock<IWorkshopService>();
        _unitOfWorkMock = new Mock<IAppointmentUnitOfWork>();
        _mapperMock = new Mock<IMapper>();

        _handler = new CreateAppointmentCommandHandler(
            _repositoryMock.Object,
            _workshopServiceMock.Object,
            _unitOfWorkMock.Object,
            _mapperMock.Object
        );
    }

    [Fact]
    public async Task Handle_WithValidCommand_ShouldReturnAppointmentId()
    {
        // Arrange
        var command = new CreateAppointmentCommand(
            PlaceId: 1,
            AppointmentAt: DateTime.UtcNow.AddDays(1),
            ServiceType: "Cambio de aceite",
            Contact: new ContactDto("Juan Pérez", "juan@example.com", "+5491123456789"),
            Vehicle: new VehicleDto("Toyota", "Corolla", 2020, "ABC123")
        );

        var workshop = new Boxes.Application.DTOs.WorkshopDto(
            1, "Taller Test", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, true, null, null, null, null, null, null, null, null, null, null, null, null
        );

        var appointment = new Appointment(
            command.PlaceId,
            command.AppointmentAt,
            command.ServiceType,
            new Contact(command.Contact.Name, command.Contact.Email, command.Contact.Phone),
            command.Vehicle != null ? new Vehicle(command.Vehicle.Make, command.Vehicle.Model, command.Vehicle.Year, command.Vehicle.LicensePlate) : null
        );
        
        // Establecer un ID válido para el appointment
        var idProperty = typeof(BaseDomainModel).GetProperty(nameof(BaseDomainModel.Id));
        idProperty?.SetValue(appointment, 1);

        _workshopServiceMock
            .Setup(x => x.GetWorkshopByIdAsync(command.PlaceId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(workshop);

        _mapperMock
            .Setup(x => x.Map<Appointment>(command))
            .Returns(appointment);

        _repositoryMock
            .Setup(x => x.AddAsync(It.IsAny<Appointment>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Appointment a, CancellationToken ct) => 
            {
                var appointmentToReturn = a;
                if (appointmentToReturn.Id == 0)
                {
                    idProperty?.SetValue(appointmentToReturn, 1);
                }
                // Asegurar que el appointment retornado tenga el ID correcto
                var returnedAppointment = new Appointment(
                    appointmentToReturn.PlaceId,
                    appointmentToReturn.AppointmentAt,
                    appointmentToReturn.ServiceType,
                    appointmentToReturn.Contact,
                    appointmentToReturn.Vehicle
                );
                idProperty?.SetValue(returnedAppointment, appointmentToReturn.Id);
                return returnedAppointment;
            });

        _unitOfWorkMock
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeGreaterThan(0);
        _workshopServiceMock.Verify(x => x.GetWorkshopByIdAsync(command.PlaceId, It.IsAny<CancellationToken>()), Times.Once);
        _repositoryMock.Verify(x => x.AddAsync(It.IsAny<Appointment>(), It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithInactiveWorkshop_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var command = new CreateAppointmentCommand(
            PlaceId: 1,
            AppointmentAt: DateTime.UtcNow.AddDays(1),
            ServiceType: "Servicio",
            Contact: new ContactDto("Test", "test@example.com", null),
            Vehicle: null
        );

        var inactiveWorkshop = new Boxes.Application.DTOs.WorkshopDto(
            1, "Taller Inactivo", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, false, null, null, null, null, null, null, null, null, null, null, null, null
        );

        _workshopServiceMock
            .Setup(x => x.GetWorkshopByIdAsync(command.PlaceId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(inactiveWorkshop);

        // Act & Assert
        var act = async () => await _handler.Handle(command, CancellationToken.None);
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage($"El taller con PlaceId {command.PlaceId} no existe o no está activo.*");
    }

    [Fact]
    public async Task Handle_WithNonExistentWorkshop_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var command = new CreateAppointmentCommand(
            PlaceId: 999,
            AppointmentAt: DateTime.UtcNow.AddDays(1),
            ServiceType: "Servicio",
            Contact: new ContactDto("Test", "test@example.com", null),
            Vehicle: null
        );

        _workshopServiceMock
            .Setup(x => x.GetWorkshopByIdAsync(command.PlaceId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Boxes.Application.DTOs.WorkshopDto?)null);

        // Act & Assert
        var act = async () => await _handler.Handle(command, CancellationToken.None);
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage($"El taller con PlaceId {command.PlaceId} no existe o no está activo.*");
    }
}

