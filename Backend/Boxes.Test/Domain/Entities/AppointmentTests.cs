using Boxes.Domain.Entities;
using FluentAssertions;

namespace Boxes.Test.Domain.Entities;

public class AppointmentTests
{
    [Fact]
    public void Constructor_WithValidData_ShouldCreateAppointment()
    {
        // Arrange
        var placeId = 1;
        var appointmentAt = DateTime.UtcNow.AddDays(1);
        var serviceType = "Cambio de aceite";
        var contact = new Contact("Juan Pérez", "juan@example.com", "+5491123456789");
        var vehicle = new Vehicle("Toyota", "Corolla", 2020, "ABC123");

        // Act
        var appointment = new Appointment(placeId, appointmentAt, serviceType, contact, vehicle);

        // Assert
        appointment.Should().NotBeNull();
        appointment.PlaceId.Should().Be(placeId);
        appointment.AppointmentAt.Should().Be(appointmentAt);
        appointment.ServiceType.Should().Be(serviceType);
        appointment.Contact.Should().Be(contact);
        appointment.Vehicle.Should().Be(vehicle);
        appointment.CreatedAt.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(1));
        appointment.UpdatedAt.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void Constructor_WithoutVehicle_ShouldCreateAppointment()
    {
        // Arrange
        var placeId = 1;
        var appointmentAt = DateTime.UtcNow.AddDays(1);
        var serviceType = "Consulta general";
        var contact = new Contact("María García", "maria@example.com");

        // Act
        var appointment = new Appointment(placeId, appointmentAt, serviceType, contact);

        // Assert
        appointment.Should().NotBeNull();
        appointment.Vehicle.Should().BeNull();
    }

    [Fact]
    public void Constructor_WithInvalidPlaceId_ShouldThrowArgumentException()
    {
        // Arrange
        var invalidPlaceId = 0;
        var appointmentAt = DateTime.UtcNow.AddDays(1);
        var serviceType = "Servicio";
        var contact = new Contact("Test", "test@example.com");

        // Act & Assert
        var act = () => new Appointment(invalidPlaceId, appointmentAt, serviceType, contact);
        act.Should().Throw<ArgumentException>()
            .WithMessage("PlaceId must be greater than 0.*");
    }

    [Fact]
    public void Constructor_WithPastDate_ShouldThrowArgumentException()
    {
        // Arrange
        var placeId = 1;
        var pastDate = DateTime.UtcNow.AddDays(-1);
        var serviceType = "Servicio";
        var contact = new Contact("Test", "test@example.com");

        // Act & Assert
        var act = () => new Appointment(placeId, pastDate, serviceType, contact);
        act.Should().Throw<ArgumentException>()
            .WithMessage("Appointment date must be in the future.*");
    }

    [Fact]
    public void Constructor_WithEmptyServiceType_ShouldThrowArgumentException()
    {
        // Arrange
        var placeId = 1;
        var appointmentAt = DateTime.UtcNow.AddDays(1);
        var emptyServiceType = string.Empty;
        var contact = new Contact("Test", "test@example.com");

        // Act & Assert
        var act = () => new Appointment(placeId, appointmentAt, emptyServiceType, contact);
        act.Should().Throw<ArgumentException>()
            .WithMessage("Service type is required.*");
    }

    [Fact]
    public void Constructor_WithNullContact_ShouldThrowArgumentNullException()
    {
        // Arrange
        var placeId = 1;
        var appointmentAt = DateTime.UtcNow.AddDays(1);
        var serviceType = "Servicio";
        Contact? contact = null;

        // Act & Assert
        var act = () => new Appointment(placeId, appointmentAt, serviceType, contact!);
        act.Should().Throw<ArgumentNullException>()
            .WithParameterName("contact");
    }
}

