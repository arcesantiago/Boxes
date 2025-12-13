using Boxes.Application.DTOs;
using Boxes.Application.Features.Appointments.Commands.CreateAppointment;
using FluentAssertions;
using FluentValidation;

namespace Boxes.Test.Application.Validators;

public class CreateAppointmentCommandValidatorTests
{
    private readonly CreateAppointmentCommandValidator _validator;

    public CreateAppointmentCommandValidatorTests()
    {
        _validator = new CreateAppointmentCommandValidator();
    }

    [Fact]
    public void Validate_WithValidCommand_ShouldPass()
    {
        // Arrange
        var command = new CreateAppointmentCommand(
            PlaceId: 1,
            AppointmentAt: DateTime.UtcNow.AddDays(1),
            ServiceType: "Cambio de aceite",
            Contact: new ContactDto("Juan Pérez", "juan@example.com", "+5491123456789"),
            Vehicle: new VehicleDto("Toyota", "Corolla", 2020, "ABC123")
        );

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_WithInvalidPlaceId_ShouldFail()
    {
        // Arrange
        var command = new CreateAppointmentCommand(
            PlaceId: 0,
            AppointmentAt: DateTime.UtcNow.AddDays(1),
            ServiceType: "Servicio",
            Contact: new ContactDto("Test", "test@example.com", null),
            Vehicle: null
        );

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "PlaceId");
    }

    [Fact]
    public void Validate_WithPastDate_ShouldFail()
    {
        // Arrange
        var command = new CreateAppointmentCommand(
            PlaceId: 1,
            AppointmentAt: DateTime.UtcNow.AddDays(-1),
            ServiceType: "Servicio",
            Contact: new ContactDto("Test", "test@example.com", null),
            Vehicle: null
        );

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "AppointmentAt");
    }

    [Fact]
    public void Validate_WithEmptyServiceType_ShouldFail()
    {
        // Arrange
        var command = new CreateAppointmentCommand(
            PlaceId: 1,
            AppointmentAt: DateTime.UtcNow.AddDays(1),
            ServiceType: string.Empty,
            Contact: new ContactDto("Test", "test@example.com", null),
            Vehicle: null
        );

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "ServiceType");
    }

    [Fact]
    public void Validate_WithNullContact_ShouldFail()
    {
        // Arrange
        var command = new CreateAppointmentCommand(
            PlaceId: 1,
            AppointmentAt: DateTime.UtcNow.AddDays(1),
            ServiceType: "Servicio",
            Contact: null!,
            Vehicle: null
        );

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        // La validación puede fallar por Contact null o por intentar acceder a Contact.Name
        result.Errors.Should().Contain(e => e.PropertyName == "Contact" || e.PropertyName.StartsWith("Contact"));
    }

    [Fact]
    public void Validate_WithInvalidEmail_ShouldFail()
    {
        // Arrange
        var command = new CreateAppointmentCommand(
            PlaceId: 1,
            AppointmentAt: DateTime.UtcNow.AddDays(1),
            ServiceType: "Servicio",
            Contact: new ContactDto("Test", "invalid-email", null),
            Vehicle: null
        );

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Contact.Email");
    }

    [Fact]
    public void Validate_WithLongServiceType_ShouldFail()
    {
        // Arrange
        var longServiceType = new string('A', 201);
        var command = new CreateAppointmentCommand(
            PlaceId: 1,
            AppointmentAt: DateTime.UtcNow.AddDays(1),
            ServiceType: longServiceType,
            Contact: new ContactDto("Test", "test@example.com", null),
            Vehicle: null
        );

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "ServiceType");
    }

    [Fact]
    public void Validate_WithEmptyContactName_ShouldFail()
    {
        // Arrange
        var command = new CreateAppointmentCommand(
            PlaceId: 1,
            AppointmentAt: DateTime.UtcNow.AddDays(1),
            ServiceType: "Servicio",
            Contact: new ContactDto(string.Empty, "test@example.com", null),
            Vehicle: null
        );

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Contact.Name");
    }

    [Fact]
    public void Validate_WithLongContactName_ShouldFail()
    {
        // Arrange
        var longName = new string('A', 201);
        var command = new CreateAppointmentCommand(
            PlaceId: 1,
            AppointmentAt: DateTime.UtcNow.AddDays(1),
            ServiceType: "Servicio",
            Contact: new ContactDto(longName, "test@example.com", null),
            Vehicle: null
        );

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Contact.Name");
    }

    [Fact]
    public void Validate_WithLongContactEmail_ShouldFail()
    {
        // Arrange
        var longEmail = new string('A', 201) + "@example.com";
        var command = new CreateAppointmentCommand(
            PlaceId: 1,
            AppointmentAt: DateTime.UtcNow.AddDays(1),
            ServiceType: "Servicio",
            Contact: new ContactDto("Test", longEmail, null),
            Vehicle: null
        );

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Contact.Email");
    }

    [Fact]
    public void Validate_WithLongContactPhone_ShouldFail()
    {
        // Arrange
        var longPhone = new string('1', 51);
        var command = new CreateAppointmentCommand(
            PlaceId: 1,
            AppointmentAt: DateTime.UtcNow.AddDays(1),
            ServiceType: "Servicio",
            Contact: new ContactDto("Test", "test@example.com", longPhone),
            Vehicle: null
        );

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Contact.Phone");
    }

    [Fact]
    public void Validate_WithLongLicensePlate_ShouldFail()
    {
        // Arrange
        var longLicensePlate = new string('A', 21);
        var command = new CreateAppointmentCommand(
            PlaceId: 1,
            AppointmentAt: DateTime.UtcNow.AddDays(1),
            ServiceType: "Servicio",
            Contact: new ContactDto("Test", "test@example.com", null),
            Vehicle: new VehicleDto("Toyota", "Corolla", 2020, longLicensePlate)
        );

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Vehicle.LicensePlate");
    }

    [Fact]
    public void Validate_WithValidVehicle_ShouldPass()
    {
        // Arrange
        var command = new CreateAppointmentCommand(
            PlaceId: 1,
            AppointmentAt: DateTime.UtcNow.AddDays(1),
            ServiceType: "Servicio",
            Contact: new ContactDto("Test", "test@example.com", null),
            Vehicle: new VehicleDto("Toyota", "Corolla", 2020, "ABC123")
        );

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeTrue();
    }
}

