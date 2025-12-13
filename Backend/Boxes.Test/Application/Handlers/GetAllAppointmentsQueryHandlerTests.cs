using AutoMapper;
using Boxes.Application.Contracts.Persistence;
using Boxes.Application.DTOs;
using Boxes.Application.Features.Appointments.Queries.GetAllAppointments;
using Boxes.Domain.Entities;
using FluentAssertions;
using Moq;
using System.Linq.Expressions;

namespace Boxes.Test.Application.Handlers;

public class GetAllAppointmentsQueryHandlerTests
{
    private readonly Mock<IAppointmentRepository> _repositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetAllAppointmentsQueryHandler _handler;

    public GetAllAppointmentsQueryHandlerTests()
    {
        _repositoryMock = new Mock<IAppointmentRepository>();
        _mapperMock = new Mock<IMapper>();

        _handler = new GetAllAppointmentsQueryHandler(
            _repositoryMock.Object,
            _mapperMock.Object
        );
    }

    [Fact]
    public async Task Handle_WithAppointments_ShouldReturnAppointmentDtos()
    {
        // Arrange
        var appointments = new List<Appointment>
        {
            new Appointment(
                1,
                DateTime.UtcNow.AddDays(1),
                "Servicio 1",
                new Contact("Test 1", "test1@example.com"),
                null
            ),
            new Appointment(
                1,
                DateTime.UtcNow.AddDays(2),
                "Servicio 2",
                new Contact("Test 2", "test2@example.com"),
                new Vehicle("Toyota", "Corolla", 2020, "ABC123")
            )
        };

        var appointmentDtos = new List<AppointmentDto>
        {
            new AppointmentDto(1, 1, DateTime.UtcNow.AddDays(1), "Servicio 1",
                new ContactDto("Test 1", "test1@example.com", null), null, DateTimeOffset.UtcNow),
            new AppointmentDto(2, 1, DateTime.UtcNow.AddDays(2), "Servicio 2",
                new ContactDto("Test 2", "test2@example.com", null),
                new VehicleDto("Toyota", "Corolla", 2020, "ABC123"), DateTimeOffset.UtcNow)
        };

        _repositoryMock
            .Setup(x => x.GetListAsync(
                It.IsAny<Expression<Func<Appointment, bool>>?>(),
                It.IsAny<Expression<Func<Appointment, Appointment>>?>(),
                It.IsAny<Func<IQueryable<Appointment>, IOrderedQueryable<Appointment>>?>(),
                It.IsAny<IEnumerable<Expression<Func<Appointment, object>>>>(),
                It.IsAny<bool>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(appointments);

        _mapperMock
            .Setup(x => x.Map<IEnumerable<AppointmentDto>>(appointments))
            .Returns(appointmentDtos);

        // Act
        var result = await _handler.Handle(new GetAllAppointmentsQuery(), CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        _repositoryMock.Verify(x => x.GetListAsync(
            It.IsAny<Expression<Func<Appointment, bool>>?>(),
            It.IsAny<Expression<Func<Appointment, Appointment>>?>(),
            It.IsAny<Func<IQueryable<Appointment>, IOrderedQueryable<Appointment>>?>(),
            It.IsAny<IEnumerable<Expression<Func<Appointment, object>>>>(),
            It.IsAny<bool>(),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithNoAppointments_ShouldReturnEmptyList()
    {
        // Arrange
        var emptyList = new List<Appointment>();

        _repositoryMock
            .Setup(x => x.GetListAsync(
                It.IsAny<Expression<Func<Appointment, bool>>?>(),
                It.IsAny<Expression<Func<Appointment, Appointment>>?>(),
                It.IsAny<Func<IQueryable<Appointment>, IOrderedQueryable<Appointment>>?>(),
                It.IsAny<IEnumerable<Expression<Func<Appointment, object>>>>(),
                It.IsAny<bool>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(emptyList);

        _mapperMock
            .Setup(x => x.Map<IEnumerable<AppointmentDto>>(emptyList))
            .Returns(new List<AppointmentDto>());

        // Act
        var result = await _handler.Handle(new GetAllAppointmentsQuery(), CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }
}

