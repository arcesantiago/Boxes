using Boxes.Domain.Entities;
using FluentAssertions;

namespace Boxes.Test.Domain.Entities;

public class ContactTests
{
    [Fact]
    public void Constructor_WithValidData_ShouldCreateContact()
    {
        // Arrange
        var name = "Juan Pérez";
        var email = "juan@example.com";
        var phone = "+5491123456789";

        // Act
        var contact = new Contact(name, email, phone);

        // Assert
        contact.Should().NotBeNull();
        contact.Name.Should().Be(name);
        contact.Email.Should().Be(email);
        contact.Phone.Should().Be(phone);
    }

    [Fact]
    public void Constructor_WithoutPhone_ShouldCreateContact()
    {
        // Arrange
        var name = "María García";
        var email = "maria@example.com";

        // Act
        var contact = new Contact(name, email);

        // Assert
        contact.Should().NotBeNull();
        contact.Phone.Should().BeNull();
    }

    [Fact]
    public void Constructor_WithEmptyName_ShouldThrowArgumentException()
    {
        // Arrange
        var emptyName = string.Empty;
        var email = "test@example.com";

        // Act & Assert
        var act = () => new Contact(emptyName, email);
        act.Should().Throw<ArgumentException>()
            .WithMessage("Name is required*");
    }

    [Fact]
    public void Constructor_WithEmptyEmail_ShouldThrowArgumentException()
    {
        // Arrange
        var name = "Test";
        var emptyEmail = string.Empty;

        // Act & Assert
        var act = () => new Contact(name, emptyEmail);
        act.Should().Throw<ArgumentException>()
            .WithMessage("Email is required*");
    }
}

