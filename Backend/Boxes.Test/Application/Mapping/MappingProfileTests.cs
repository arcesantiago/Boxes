using AutoMapper;
using Boxes.Application.DTOs;
using Boxes.Application.Features.Appointments.Commands.CreateAppointment;
using Boxes.Application.Mapping;
using Boxes.Domain.Entities;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;

namespace Boxes.Test.Application.Mapping;

public class MappingProfileTests
{
    private readonly IMapper _mapper;

    public MappingProfileTests()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>(), NullLoggerFactory.Instance);
        _mapper = config.CreateMapper();
    }

    [Fact]
    public void MappingProfile_ShouldBeValid()
    {
        // Arrange & Act
        var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>(), NullLoggerFactory.Instance);

        // Assert
        config.AssertConfigurationIsValid();
    }

    [Fact]
    public void Map_CreateAppointmentCommandToAppointment_ShouldMapCorrectly()
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
        var appointment = _mapper.Map<Appointment>(command);

        // Assert
        appointment.Should().NotBeNull();
        appointment.PlaceId.Should().Be(command.PlaceId);
        appointment.AppointmentAt.Should().Be(command.AppointmentAt);
        appointment.ServiceType.Should().Be(command.ServiceType);
        appointment.Contact.Name.Should().Be(command.Contact.Name);
        appointment.Contact.Email.Should().Be(command.Contact.Email);
        appointment.Vehicle.Should().NotBeNull();
        appointment.Vehicle!.Make.Should().Be(command.Vehicle!.Make);
    }

    [Fact]
    public void Map_AppointmentToAppointmentDto_ShouldMapCorrectly()
    {
        // Arrange
        var appointment = new Appointment(
            1,
            DateTime.UtcNow.AddDays(1),
            "Cambio de aceite",
            new Contact("Juan Pérez", "juan@example.com", "+5491123456789"),
            new Vehicle("Toyota", "Corolla", 2020, "ABC123")
        );

        // Act
        var dto = _mapper.Map<AppointmentDto>(appointment);

        // Assert
        dto.Should().NotBeNull();
        dto.PlaceId.Should().Be(appointment.PlaceId);
        dto.AppointmentAt.Should().Be(appointment.AppointmentAt);
        dto.ServiceType.Should().Be(appointment.ServiceType);
        dto.Contact.Name.Should().Be(appointment.Contact.Name);
        dto.Contact.Email.Should().Be(appointment.Contact.Email);
        dto.Vehicle.Should().NotBeNull();
        dto.Vehicle!.Make.Should().Be(appointment.Vehicle!.Make);
    }

    [Fact]
    public void Map_ContactToContactDto_ShouldMapCorrectly()
    {
        // Arrange
        var contact = new Contact("Juan Pérez", "juan@example.com", "+5491123456789");

        // Act
        var dto = _mapper.Map<ContactDto>(contact);

        // Assert
        dto.Should().NotBeNull();
        dto.Name.Should().Be(contact.Name);
        dto.Email.Should().Be(contact.Email);
        dto.Phone.Should().Be(contact.Phone);
    }

    [Fact]
    public void Map_VehicleToVehicleDto_ShouldMapCorrectly()
    {
        // Arrange
        var vehicle = new Vehicle("Toyota", "Corolla", 2020, "ABC123");

        // Act
        var dto = _mapper.Map<VehicleDto>(vehicle);

        // Assert
        dto.Should().NotBeNull();
        dto.Make.Should().Be(vehicle.Make);
        dto.Model.Should().Be(vehicle.Model);
        dto.Year.Should().Be(vehicle.Year);
        dto.LicensePlate.Should().Be(vehicle.LicensePlate);
    }

    [Fact]
    public void Map_WorkshopDtoToWorkshopDto_ShouldMapAllFields()
    {
        // Arrange
        var source = new WorkshopDto(
            Id: 1,
            Name: "Taller Test",
            Email: "test@example.com",
            Email2: null,
            Phone: "+5491123456789",
            Phone2: null,
            Phone3: null,
            Address: null,
            DefaultAddress: null,
            Website: "https://test.com",
            SocialFacebook: null,
            SocialTwitter: null,
            SocialLinkedIn: null,
            TimeZone: null,
            Schedules: null,
            Relationships: null,
            Type: "Workshop",
            CreatedAt: DateTime.UtcNow,
            UpdatedAt: DateTime.UtcNow,
            RemovedAt: null,
            Group: null,
            FormattedAddress: "Test Address",
            Active: true,
            DefaultFormattedAddress: null,
            AreaCode: null,
            CountryCode: null,
            ZoneInformation: null,
            MakeCode: null,
            TimePerShift: null,
            AmountPerShift: null,
            MaximumPerDay: null,
            MinimumAnticipationDays: null,
            ExternalsCrm: null,
            Externals: null,
            ResourceType: null
        );

        // Act
        var result = _mapper.Map<WorkshopDto>(source);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(source.Id);
        result.Name.Should().Be(source.Name);
        result.Email.Should().Be(source.Email);
        result.Phone.Should().Be(source.Phone);
        result.Active.Should().Be(source.Active);
    }

    [Fact]
    public void Map_WorkshopDtoToWorkshopDto_ShouldMapActiveToActive()
    {
        // Arrange
        var source = new WorkshopDto(
            Id: 1,
            Name: "Taller Test",
            Email: null,
            Email2: null,
            Phone: null,
            Phone2: null,
            Phone3: null,
            Address: null,
            DefaultAddress: null,
            Website: null,
            SocialFacebook: null,
            SocialTwitter: null,
            SocialLinkedIn: null,
            TimeZone: null,
            Schedules: null,
            Relationships: null,
            Type: null,
            CreatedAt: null,
            UpdatedAt: null,
            RemovedAt: null,
            Group: null,
            FormattedAddress: null,
            Active: true,
            DefaultFormattedAddress: null,
            AreaCode: null,
            CountryCode: null,
            ZoneInformation: null,
            MakeCode: null,
            TimePerShift: null,
            AmountPerShift: null,
            MaximumPerDay: null,
            MinimumAnticipationDays: null,
            ExternalsCrm: null,
            Externals: null,
            ResourceType: null
        );

        // Act
        var result = _mapper.Map<WorkshopDto>(source);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(source.Id);
        result.Active.Should().Be(source.Active);
    }
}

