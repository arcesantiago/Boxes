using Boxes.Application.Behaviours;
using Boxes.Application.DTOs;
using Boxes.Application.Features.Appointments.Commands.CreateAppointment;
using FluentAssertions;
using FluentValidation;
using MediatR;
using Moq;

namespace Boxes.Test.Application.Behaviours;

public class ValidationBehaviourTests
{
    private readonly Mock<IValidator<CreateAppointmentCommand>> _validatorMock;
    private readonly ValidationBehaviour<CreateAppointmentCommand, int> _behaviour;
    private readonly RequestHandlerDelegate<int> _next;

    public ValidationBehaviourTests()
    {
        _validatorMock = new Mock<IValidator<CreateAppointmentCommand>>();
        _behaviour = new ValidationBehaviour<CreateAppointmentCommand, int>(new[] { _validatorMock.Object });
        _next = ct => Task.FromResult(1);
    }

    [Fact]
    public async Task Handle_WithValidRequest_ShouldCallNext()
    {
        // Arrange
        var request = new CreateAppointmentCommand(
            PlaceId: 1,
            AppointmentAt: DateTime.UtcNow.AddDays(1),
            ServiceType: "Servicio",
            Contact: new ContactDto("Test", "test@example.com", null),
            Vehicle: null
        );

        var validationResult = new FluentValidation.Results.ValidationResult(new List<FluentValidation.Results.ValidationFailure>());
        _validatorMock
            .Setup(x => x.ValidateAsync(It.IsAny<ValidationContext<CreateAppointmentCommand>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        // Act
        var result = await _behaviour.Handle(request, _next, CancellationToken.None);

        // Assert
        result.Should().Be(1);
        _validatorMock.Verify(x => x.ValidateAsync(It.IsAny<ValidationContext<CreateAppointmentCommand>>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithInvalidRequest_ShouldThrowValidationException()
    {
        // Arrange
        var request = new CreateAppointmentCommand(
            PlaceId: 0, // Invalid
            AppointmentAt: DateTime.UtcNow.AddDays(1),
            ServiceType: "Servicio",
            Contact: new ContactDto("Test", "test@example.com", null),
            Vehicle: null
        );

        var failures = new List<FluentValidation.Results.ValidationFailure>
        {
            new FluentValidation.Results.ValidationFailure("PlaceId", "PlaceId debe ser mayor a 0")
        };
        var validationResult = new FluentValidation.Results.ValidationResult(failures);
        _validatorMock
            .Setup(x => x.ValidateAsync(It.IsAny<ValidationContext<CreateAppointmentCommand>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        // Act & Assert
        var act = async () => await _behaviour.Handle(request, _next, CancellationToken.None);
        await act.Should().ThrowAsync<Boxes.Application.Exceptions.ValidationException>();
    }
}

