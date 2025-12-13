using Boxes.Application.Behaviours;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;

namespace Boxes.Test.Application.Behaviours;

public class UnhandledExceptionBehaviourTests
{
    private readonly Mock<RequestHandlerDelegate<int>> _nextMock;
    private readonly Mock<ILogger<IRequest<int>>> _loggerMock;
    private readonly UnhandledExceptionBehaviour<IRequest<int>, int> _behaviour;

    public UnhandledExceptionBehaviourTests()
    {
        _nextMock = new Mock<RequestHandlerDelegate<int>>();
        _loggerMock = new Mock<ILogger<IRequest<int>>>();
        _behaviour = new UnhandledExceptionBehaviour<IRequest<int>, int>(_loggerMock.Object);
    }

    [Fact]
    public async Task Handle_WithException_ShouldLogAndRethrow()
    {
        // Arrange
        var request = new Mock<IRequest<int>>();
        var exception = new InvalidOperationException("Test exception");

        _nextMock
            .Setup(x => x())
            .ThrowsAsync(exception);

        // Act & Assert
        var act = async () => await _behaviour.Handle(request.Object, _nextMock.Object, CancellationToken.None);
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Test exception");
        
        // Verificar que se logueó el error
        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WithoutException_ShouldReturnResult()
    {
        // Arrange
        var request = new Mock<IRequest<int>>();
        var expectedResult = 42;

        _nextMock
            .Setup(x => x())
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _behaviour.Handle(request.Object, _nextMock.Object, CancellationToken.None);

        // Assert
        result.Should().Be(expectedResult);
        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)),
            Times.Never);
    }
}

