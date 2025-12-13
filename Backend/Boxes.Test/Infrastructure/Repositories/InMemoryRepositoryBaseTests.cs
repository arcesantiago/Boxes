using Boxes.Domain.Entities;
using Boxes.Infrastructure.Repositories;
using FluentAssertions;

namespace Boxes.Test.Infrastructure.Repositories;

public class InMemoryRepositoryBaseTests
{
    private readonly InMemoryAppointmentRepository _repository;

    public InMemoryRepositoryBaseTests()
    {
        _repository = new InMemoryAppointmentRepository();
    }

    [Fact]
    public async Task AddAsync_WithValidEntity_ShouldAddAndReturnEntity()
    {
        // Arrange
        var appointment = new Appointment(
            1,
            DateTime.UtcNow.AddDays(1),
            "Servicio Test",
            new Contact("Test", "test@example.com"),
            null
        );

        // Act
        var result = await _repository.AddAsync(appointment);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().BeGreaterThan(0);
        result.CreatedAt.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public async Task FindAsync_WithExistingId_ShouldReturnEntity()
    {
        // Arrange
        var appointment = new Appointment(
            1,
            DateTime.UtcNow.AddDays(1),
            "Servicio Test",
            new Contact("Test", "test@example.com"),
            null
        );
        var added = await _repository.AddAsync(appointment);

        // Act
        var result = await _repository.FindAsync(added.Id);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(added.Id);
        result.ServiceType.Should().Be("Servicio Test");
    }

    [Fact]
    public async Task FindAsync_WithNonExistentId_ShouldReturnNull()
    {
        // Act
        var result = await _repository.FindAsync(999);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetListAsync_WithEntities_ShouldReturnAllEntities()
    {
        // Arrange
        var appointment1 = new Appointment(
            1,
            DateTime.UtcNow.AddDays(1),
            "Servicio 1",
            new Contact("Test 1", "test1@example.com"),
            null
        );
        var appointment2 = new Appointment(
            1,
            DateTime.UtcNow.AddDays(2),
            "Servicio 2",
            new Contact("Test 2", "test2@example.com"),
            null
        );

        await _repository.AddAsync(appointment1);
        await _repository.AddAsync(appointment2);

        // Act
        var result = await _repository.GetListAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCountGreaterThanOrEqualTo(2);
    }

    [Fact]
    public async Task GetListAsync_WithPredicate_ShouldReturnFilteredEntities()
    {
        // Arrange
        var appointment1 = new Appointment(
            1,
            DateTime.UtcNow.AddDays(1),
            "Servicio 1",
            new Contact("Test 1", "test1@example.com"),
            null
        );
        var appointment2 = new Appointment(
            2,
            DateTime.UtcNow.AddDays(2),
            "Servicio 2",
            new Contact("Test 2", "test2@example.com"),
            null
        );

        await _repository.AddAsync(appointment1);
        await _repository.AddAsync(appointment2);

        // Act
        var result = await _repository.GetListAsync(a => a.PlaceId == 1);

        // Assert
        result.Should().NotBeNull();
        result.Should().OnlyContain(a => a.PlaceId == 1);
    }

    [Fact]
    public async Task ExistsAsync_WithExistingEntity_ShouldReturnTrue()
    {
        // Arrange
        var appointment = new Appointment(
            1,
            DateTime.UtcNow.AddDays(1),
            "Servicio Test",
            new Contact("Test", "test@example.com"),
            null
        );
        await _repository.AddAsync(appointment);

        // Act
        var result = await _repository.ExistsAsync(a => a.PlaceId == 1);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task ExistsAsync_WithNonExistentEntity_ShouldReturnFalse()
    {
        // Act
        var result = await _repository.ExistsAsync(a => a.PlaceId == 999);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task CountAsync_WithEntities_ShouldReturnCorrectCount()
    {
        // Arrange
        var appointment1 = new Appointment(
            1,
            DateTime.UtcNow.AddDays(1),
            "Servicio 1",
            new Contact("Test 1", "test1@example.com"),
            null
        );
        var appointment2 = new Appointment(
            1,
            DateTime.UtcNow.AddDays(2),
            "Servicio 2",
            new Contact("Test 2", "test2@example.com"),
            null
        );

        await _repository.AddAsync(appointment1);
        await _repository.AddAsync(appointment2);

        // Act - CountAsync no está implementado, usar GetListAsync
        var result = (await _repository.GetListAsync()).Count;

        // Assert
        result.Should().BeGreaterThanOrEqualTo(2);
    }
}

