using Boxes.Domain.Common;
using Boxes.Domain.Entities;
using Boxes.Infrastructure.Repositories;
using FluentAssertions;
using System.Linq.Expressions;

namespace Boxes.Test.Infrastructure.Repositories;

public class InMemoryRepositoryBaseAdditionalTests
{
    private readonly InMemoryAppointmentRepository _repository;

    public InMemoryRepositoryBaseAdditionalTests()
    {
        _repository = new InMemoryAppointmentRepository();
    }

    [Fact]
    public async Task GetListAsync_WithOrderBy_ShouldReturnOrderedResults()
    {
        // Arrange
        var appointment1 = new Appointment(
            1,
            DateTime.UtcNow.AddDays(3),
            "Servicio 3",
            new Contact("Test 3", "test3@example.com"),
            null
        );
        var appointment2 = new Appointment(
            1,
            DateTime.UtcNow.AddDays(1),
            "Servicio 1",
            new Contact("Test 1", "test1@example.com"),
            null
        );
        var appointment3 = new Appointment(
            1,
            DateTime.UtcNow.AddDays(2),
            "Servicio 2",
            new Contact("Test 2", "test2@example.com"),
            null
        );

        await _repository.AddAsync(appointment1);
        await _repository.AddAsync(appointment2);
        await _repository.AddAsync(appointment3);

        // Act - Ordenar por ServiceType ascendente
        var result = await _repository.GetListAsync(
            orderBy: q => q.OrderBy(a => a.ServiceType));

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(3);
        result.First().ServiceType.Should().Be("Servicio 1");
        result.Last().ServiceType.Should().Be("Servicio 3");
    }

    [Fact]
    public async Task GetListAsync_WithSelect_ShouldReturnProjectedResults()
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

        // Act - Seleccionar solo ServiceType
        var result = await _repository.GetListAsync(
            select: a => new Appointment(
                a.PlaceId,
                a.AppointmentAt,
                a.ServiceType,
                a.Contact,
                a.Vehicle
            ));

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCountGreaterThan(0);
    }

    [Fact]
    public async Task GetListAsync_WithEmptyStore_ShouldReturnEmptyList()
    {
        // Act
        var result = await _repository.GetListAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task AddAsync_WithExistingId_ShouldNotOverrideId()
    {
        // Arrange
        var appointment1 = new Appointment(
            1,
            DateTime.UtcNow.AddDays(1),
            "Servicio 1",
            new Contact("Test 1", "test1@example.com"),
            null
        );
        var added1 = await _repository.AddAsync(appointment1);
        var firstId = added1.Id;

        var appointment2 = new Appointment(
            1,
            DateTime.UtcNow.AddDays(2),
            "Servicio 2",
            new Contact("Test 2", "test2@example.com"),
            null
        );

        // Act
        var added2 = await _repository.AddAsync(appointment2);

        // Assert
        added2.Id.Should().NotBe(firstId);
        added2.Id.Should().BeGreaterThan(firstId);
    }

    [Fact]
    public async Task AddAsync_WithPreSetId_ShouldUseProvidedId()
    {
        // Arrange
        var appointment = new Appointment(
            1,
            DateTime.UtcNow.AddDays(1),
            "Servicio Test",
            new Contact("Test", "test@example.com"),
            null
        );
        
        // Establecer ID manualmente usando reflexión
        var idProperty = typeof(BaseDomainModel).GetProperty(nameof(BaseDomainModel.Id));
        idProperty?.SetValue(appointment, 999);

        // Act
        var result = await _repository.AddAsync(appointment);

        // Assert
        result.Id.Should().Be(999);
    }

    [Fact]
    public async Task GetListAsync_WithComplexPredicate_ShouldFilterCorrectly()
    {
        // Arrange
        var appointment1 = new Appointment(
            1,
            DateTime.UtcNow.AddDays(1),
            "Cambio de aceite",
            new Contact("Test 1", "test1@example.com"),
            null
        );
        var appointment2 = new Appointment(
            2,
            DateTime.UtcNow.AddDays(2),
            "Alineación",
            new Contact("Test 2", "test2@example.com"),
            null
        );
        var appointment3 = new Appointment(
            1,
            DateTime.UtcNow.AddDays(3),
            "Cambio de filtro",
            new Contact("Test 3", "test3@example.com"),
            null
        );

        await _repository.AddAsync(appointment1);
        await _repository.AddAsync(appointment2);
        await _repository.AddAsync(appointment3);

        // Act - Filtrar por PlaceId = 1 y ServiceType contiene "Cambio"
        var result = await _repository.GetListAsync(
            predicate: a => a.PlaceId == 1 && a.ServiceType.Contains("Cambio"));

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.Should().OnlyContain(a => a.PlaceId == 1 && a.ServiceType.Contains("Cambio"));
    }
}

