using Boxes.API.Controllers;
using Boxes.Application.DTOs;
using Boxes.Application.Features.Appointments.Queries.GetAllWorkshops;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Boxes.Test.API.Controllers;

public class WorkshopsControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly WorkshopsController _controller;

    public WorkshopsControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new WorkshopsController(_mediatorMock.Object);
    }

    [Fact]
    public async Task GetAllWorkshops_WithWorkshops_ShouldReturnOk()
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

        _mediatorMock
            .Setup(x => x.Send(It.IsAny<GetAllWorkshopsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(workshops);

        // Act
        var result = await _controller.GetAllWorkshops(CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.Value.Should().BeEquivalentTo(workshops);
    }

    [Fact]
    public async Task GetAllWorkshops_WithNoWorkshops_ShouldReturnEmptyList()
    {
        // Arrange
        var emptyList = new List<WorkshopDto>();

        _mediatorMock
            .Setup(x => x.Send(It.IsAny<GetAllWorkshopsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(emptyList);

        // Act
        var result = await _controller.GetAllWorkshops(CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        var value = okResult.Value.Should().BeAssignableTo<IEnumerable<WorkshopDto>>().Subject;
        value.Should().BeEmpty();
    }
}

