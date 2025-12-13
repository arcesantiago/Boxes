using Boxes.Application.Exceptions;
using FluentAssertions;
using FluentValidation.Results;

namespace Boxes.Test.Application.Exceptions;

public class ValidationExceptionTests
{
    [Fact]
    public void Constructor_WithoutParameters_ShouldCreateExceptionWithDefaultMessage()
    {
        // Act
        var exception = new ValidationException();

        // Assert
        exception.Should().NotBeNull();
        exception.Message.Should().Be("Se presentaron uno o mas errores de validacion");
        exception.Errors.Should().NotBeNull();
        exception.Errors.Should().BeEmpty();
    }

    [Fact]
    public void Constructor_WithFailures_ShouldGroupErrorsByPropertyName()
    {
        // Arrange
        var failures = new List<ValidationFailure>
        {
            new ValidationFailure("PlaceId", "PlaceId debe ser mayor a 0"),
            new ValidationFailure("PlaceId", "PlaceId es requerido"),
            new ValidationFailure("ServiceType", "ServiceType es requerido"),
            new ValidationFailure("Contact.Email", "Email inválido")
        };

        // Act
        var exception = new ValidationException(failures);

        // Assert
        exception.Should().NotBeNull();
        exception.Errors.Should().HaveCount(3); // PlaceId, ServiceType, Contact.Email
        exception.Errors["PlaceId"].Should().HaveCount(2);
        exception.Errors["ServiceType"].Should().HaveCount(1);
        exception.Errors["Contact.Email"].Should().HaveCount(1);
    }

    [Fact]
    public void Constructor_WithEmptyFailures_ShouldCreateEmptyErrors()
    {
        // Arrange
        var failures = new List<ValidationFailure>();

        // Act
        var exception = new ValidationException(failures);

        // Assert
        exception.Should().NotBeNull();
        exception.Errors.Should().BeEmpty();
    }
}

