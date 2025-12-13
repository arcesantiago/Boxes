using Boxes.Application.Behaviours;
using Boxes.Application.Common.Interfaces;
using Boxes.Application.Contracts.Infrastructure;
using Boxes.Application.DTOs;
using Boxes.Application.Features.Appointments.Queries.GetAllAppointments;
using Boxes.Application.Features.Appointments.Queries.GetAllWorkshops;
using FluentAssertions;
using MediatR;
using Moq;

namespace Boxes.Test.Application.Behaviours;

public class CachingBehaviorTests
{
    private readonly Mock<ICacheService> _cacheMock;
    private readonly Mock<RequestHandlerDelegate<IEnumerable<WorkshopDto>>> _nextMock;
    private readonly CachingBehavior<GetAllWorkshopsQuery, IEnumerable<WorkshopDto>> _behaviour;

    public CachingBehaviorTests()
    {
        _cacheMock = new Mock<ICacheService>();
        _nextMock = new Mock<RequestHandlerDelegate<IEnumerable<WorkshopDto>>>();
        _behaviour = new CachingBehavior<GetAllWorkshopsQuery, IEnumerable<WorkshopDto>>(_cacheMock.Object);
    }

    [Fact]
    public async Task Handle_WithCacheableQueryAndCachedValue_ShouldReturnCachedValue()
    {
        // Arrange
        var request = new GetAllWorkshopsQuery();
        var cachedValue = new List<WorkshopDto>
        {
            new WorkshopDto(
                1, "Taller 1", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, true, null, null, null, null, null, null, null, null, null, null, null, null
            )
        };

        _cacheMock
            .Setup(x => x.GetAsync<IEnumerable<WorkshopDto>>(request.CacheKey))
            .ReturnsAsync(cachedValue);

        // Act
        var result = await _behaviour.Handle(request, _nextMock.Object, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(cachedValue);
        _nextMock.Verify(x => x(), Times.Never);
        _cacheMock.Verify(x => x.GetAsync<IEnumerable<WorkshopDto>>(request.CacheKey), Times.Once);
    }

    [Fact]
    public async Task Handle_WithCacheableQueryAndNoCachedValue_ShouldCallNextAndCacheResult()
    {
        // Arrange
        var request = new GetAllWorkshopsQuery();
        var freshValue = new List<WorkshopDto>
        {
            new WorkshopDto(
                1, "Taller 1", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, true, null, null, null, null, null, null, null, null, null, null, null, null
            )
        };

        _cacheMock
            .Setup(x => x.GetAsync<IEnumerable<WorkshopDto>>(request.CacheKey))
            .ReturnsAsync((IEnumerable<WorkshopDto>?)null);

        _nextMock
            .Setup(x => x())
            .ReturnsAsync(freshValue);

        // Act
        var result = await _behaviour.Handle(request, _nextMock.Object, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(freshValue);
        _nextMock.Verify(x => x(), Times.Once);
        _cacheMock.Verify(x => x.SetAsync(
            request.CacheKey, 
            It.IsAny<IEnumerable<WorkshopDto>>(), 
            request.Expiration ?? TimeSpan.FromMinutes(5)), Times.Once);
    }

    [Fact]
    public async Task Handle_WithNonCacheableQuery_ShouldCallNextWithoutCaching()
    {
        // Arrange
        var request = new GetAllAppointmentsQuery();
        var freshValue = new List<AppointmentDto>();
        var behaviour = new CachingBehavior<GetAllAppointmentsQuery, IEnumerable<AppointmentDto>>(_cacheMock.Object);
        var nextMock = new Mock<RequestHandlerDelegate<IEnumerable<AppointmentDto>>>();

        nextMock
            .Setup(x => x())
            .ReturnsAsync(freshValue);

        // Act
        var result = await behaviour.Handle(request, nextMock.Object, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        nextMock.Verify(x => x(), Times.Once);
        _cacheMock.Verify(x => x.GetAsync<IEnumerable<WorkshopDto>>(It.IsAny<string>()), Times.Never);
        _cacheMock.Verify(x => x.SetAsync(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<TimeSpan>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WithCacheableQueryAndDefaultExpiration_ShouldUseDefaultExpiration()
    {
        // Arrange
        var request = new GetAllWorkshopsQuery();
        var freshValue = new List<WorkshopDto>
        {
            new WorkshopDto(
                1, "Taller 1", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, true, null, null, null, null, null, null, null, null, null, null, null, null
            )
        };

        _cacheMock
            .Setup(x => x.GetAsync<IEnumerable<WorkshopDto>>(request.CacheKey))
            .ReturnsAsync((IEnumerable<WorkshopDto>?)null);

        _nextMock
            .Setup(x => x())
            .ReturnsAsync(freshValue);

        // Act
        var result = await _behaviour.Handle(request, _nextMock.Object, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        _cacheMock.Verify(x => x.SetAsync(
            request.CacheKey, 
            It.IsAny<IEnumerable<WorkshopDto>>(), 
            request.Expiration ?? TimeSpan.FromMinutes(5)), Times.Once);
    }
}

