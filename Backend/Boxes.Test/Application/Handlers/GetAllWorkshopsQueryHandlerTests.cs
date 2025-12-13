using AutoMapper;
using Boxes.Application.Contracts.Interfaces;
using Boxes.Application.DTOs;
using Boxes.Application.Features.Appointments.Queries.GetAllWorkshops;
using FluentAssertions;
using Moq;

namespace Boxes.Test.Application.Handlers;

public class GetAllWorkshopsQueryHandlerTests
{
    private readonly Mock<IWorkshopService> _workshopServiceMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetAllWorkshopsQueryHandler _handler;

    public GetAllWorkshopsQueryHandlerTests()
    {
        _workshopServiceMock = new Mock<IWorkshopService>();
        _mapperMock = new Mock<IMapper>();

        _handler = new GetAllWorkshopsQueryHandler(
            _workshopServiceMock.Object,
            _mapperMock.Object
        );
    }

    [Fact]
    public async Task Handle_WithWorkshops_ShouldReturnWorkshopDtos()
    {
        // Arrange
        var workshops = new List<WorkshopDto>
        {
            new WorkshopDto(
                1, "Taller 1", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, true, null, null, null, null, null, null, null, null, null, null, null, null
            ),
            new WorkshopDto(
                2, "Taller 2", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, true, null, null, null, null, null, null, null, null, null, null, null, null
            )
        };

        _workshopServiceMock
            .Setup(x => x.GetActiveWorkshopsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(workshops);

        _mapperMock
            .Setup(x => x.Map<IEnumerable<WorkshopDto>>(workshops))
            .Returns(workshops);

        // Act
        var result = await _handler.Handle(new GetAllWorkshopsQuery(), CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        _workshopServiceMock.Verify(x => x.GetActiveWorkshopsAsync(It.IsAny<CancellationToken>()), Times.Once);
        _mapperMock.Verify(x => x.Map<IEnumerable<WorkshopDto>>(workshops), Times.Once);
    }

    [Fact]
    public async Task Handle_WithNoWorkshops_ShouldReturnEmptyList()
    {
        // Arrange
        var emptyList = new List<WorkshopDto>();

        _workshopServiceMock
            .Setup(x => x.GetActiveWorkshopsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(emptyList);

        _mapperMock
            .Setup(x => x.Map<IEnumerable<WorkshopDto>>(emptyList))
            .Returns(emptyList);

        // Act
        var result = await _handler.Handle(new GetAllWorkshopsQuery(), CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }
}

