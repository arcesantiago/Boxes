using Boxes.Application.Exceptions;
using FluentAssertions;

namespace Boxes.Test.Application.Exceptions;

public class NotFoundExceptionTests
{
    [Fact]
    public void Constructor_WithNameAndKey_ShouldCreateExceptionWithCorrectMessage()
    {
        // Arrange
        var name = "Appointment";
        var key = 123;

        // Act
        var exception = new NotFoundException(name, key);

        // Assert
        exception.Should().NotBeNull();
        exception.Message.Should().Be($"Entity \"{name}\" ({key}) no fue encontrada");
    }

    [Fact]
    public void Constructor_WithStringKey_ShouldCreateExceptionWithCorrectMessage()
    {
        // Arrange
        var name = "Workshop";
        var key = "test-key";

        // Act
        var exception = new NotFoundException(name, key);

        // Assert
        exception.Should().NotBeNull();
        exception.Message.Should().Be($"Entity \"{name}\" ({key}) no fue encontrada");
    }
}




