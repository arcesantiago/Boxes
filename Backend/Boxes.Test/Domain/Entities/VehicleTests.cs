using Boxes.Domain.Entities;
using FluentAssertions;

namespace Boxes.Test.Domain.Entities;

public class VehicleTests
{
    [Fact]
    public void Constructor_WithValidData_ShouldCreateVehicle()
    {
        // Arrange
        var make = "Toyota";
        var model = "Corolla";
        var year = 2020;
        var licensePlate = "ABC123";

        // Act
        var vehicle = new Vehicle(make, model, year, licensePlate);

        // Assert
        vehicle.Should().NotBeNull();
        vehicle.Make.Should().Be(make);
        vehicle.Model.Should().Be(model);
        vehicle.Year.Should().Be(year);
        vehicle.LicensePlate.Should().Be(licensePlate);
    }

    [Fact]
    public void Constructor_WithNullValues_ShouldCreateVehicle()
    {
        // Arrange
        string? make = null;
        string? model = null;
        int? year = null;
        string? licensePlate = null;

        // Act
        var vehicle = new Vehicle(make, model, year, licensePlate);

        // Assert
        vehicle.Should().NotBeNull();
        vehicle.Make.Should().BeNull();
        vehicle.Model.Should().BeNull();
        vehicle.Year.Should().BeNull();
        vehicle.LicensePlate.Should().BeNull();
    }
}

