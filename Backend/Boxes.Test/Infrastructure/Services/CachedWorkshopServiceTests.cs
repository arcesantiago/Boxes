using Boxes.Application.Contracts.Interfaces;
using Boxes.Application.DTOs;
using Boxes.Infrastructure.Services;
using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Moq;

namespace Boxes.Test.Infrastructure.Services;

public class CachedWorkshopServiceTests
{
    private readonly Mock<IWorkshopService> _innerServiceMock;
    private readonly IMemoryCache _cache;
    private readonly CachedWorkshopService _cachedService;

    public CachedWorkshopServiceTests()
    {
        _innerServiceMock = new Mock<IWorkshopService>();
        _cache = new MemoryCache(new MemoryCacheOptions());
        _cachedService = new CachedWorkshopService(_innerServiceMock.Object, _cache);
    }

    [Fact]
    public async Task GetActiveWorkshopsAsync_FirstCall_ShouldCallInnerService()
    {
        // Arrange
        var workshops = new List<WorkshopDto>
        {
            new WorkshopDto(
                1, "Taller 1", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, true, null, null, null, null, null, null, null, null, null, null, null, null
            )
        };

        _innerServiceMock
            .Setup(x => x.GetActiveWorkshopsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(workshops);

        // Act
        var result = await _cachedService.GetActiveWorkshopsAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        _innerServiceMock.Verify(x => x.GetActiveWorkshopsAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetActiveWorkshopsAsync_SecondCall_ShouldUseCache()
    {
        // Arrange
        var workshops = new List<WorkshopDto>
        {
            new WorkshopDto(
                1, "Taller 1", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, true, null, null, null, null, null, null, null, null, null, null, null, null
            )
        };

        _innerServiceMock
            .Setup(x => x.GetActiveWorkshopsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(workshops);

        // Act - Primera llamada
        await _cachedService.GetActiveWorkshopsAsync();
        
        // Segunda llamada - debería usar caché
        var result = await _cachedService.GetActiveWorkshopsAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        // Solo se debe llamar una vez al servicio interno (la segunda usa caché)
        _innerServiceMock.Verify(x => x.GetActiveWorkshopsAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetWorkshopByIdAsync_WithExistingId_ShouldReturnWorkshop()
    {
        // Arrange
        var workshops = new List<WorkshopDto>
        {
            new WorkshopDto(
                1, "Taller 1", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, true, null, null, null, null, null, null, null, null, null, null, null, null
            )
        };

        _innerServiceMock
            .Setup(x => x.GetActiveWorkshopsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(workshops);

        // Act
        var result = await _cachedService.GetWorkshopByIdAsync(1);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(1);
    }

    [Fact]
    public async Task GetWorkshopByIdAsync_WithNonExistentId_ShouldReturnNull()
    {
        // Arrange
        var workshops = new List<WorkshopDto>
        {
            new WorkshopDto(
                1, "Taller 1", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, true, null, null, null, null, null, null, null, null, null, null, null, null
            )
        };

        _innerServiceMock
            .Setup(x => x.GetActiveWorkshopsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(workshops);

        // Act
        var result = await _cachedService.GetWorkshopByIdAsync(999);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task InvalidateCache_ShouldRemoveCache()
    {
        // Arrange
        var workshops = new List<WorkshopDto>
        {
            new WorkshopDto(
                1, "Taller 1", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, true, null, null, null, null, null, null, null, null, null, null, null, null
            )
        };

        _innerServiceMock
            .Setup(x => x.GetActiveWorkshopsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(workshops);

        // Act - Primera llamada para llenar caché
        await _cachedService.GetActiveWorkshopsAsync();
        
        // Invalidar caché
        _cachedService.InvalidateCache();
        
        // Segunda llamada después de invalidar
        await _cachedService.GetActiveWorkshopsAsync();

        // Assert - Debe llamar al servicio interno dos veces (una antes y una después de invalidar)
        _innerServiceMock.Verify(x => x.GetActiveWorkshopsAsync(It.IsAny<CancellationToken>()), Times.Exactly(2));
    }
}

