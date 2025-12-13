using Boxes.Infrastructure.Repositories;
using FluentAssertions;

namespace Boxes.Test.Infrastructure.Persistence;

public class InMemoryAppointmentUnitOfWorkTests
{
    [Fact]
    public async Task SaveChangesAsync_ShouldReturnOne()
    {
        // Arrange
        var unitOfWork = new InMemoryAppointmentUnitOfWork();

        // Act
        var result = await unitOfWork.SaveChangesAsync();

        // Assert
        result.Should().Be(1);
    }

    [Fact]
    public void Dispose_ShouldNotThrow()
    {
        // Arrange
        var unitOfWork = new InMemoryAppointmentUnitOfWork();

        // Act & Assert
        var act = () => unitOfWork.Dispose();
        act.Should().NotThrow();
    }

    [Fact]
    public void Dispose_MultipleTimes_ShouldNotThrow()
    {
        // Arrange
        var unitOfWork = new InMemoryAppointmentUnitOfWork();

        // Act & Assert
        unitOfWork.Dispose();
        var act = () => unitOfWork.Dispose();
        act.Should().NotThrow();
    }
}

