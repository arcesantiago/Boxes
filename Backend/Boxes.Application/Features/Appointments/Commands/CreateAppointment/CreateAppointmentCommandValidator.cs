using FluentValidation;

namespace Boxes.Application.Features.Appointments.Commands.CreateAppointment
{
    public class CreateAppointmentCommandValidator : AbstractValidator<CreateAppointmentCommand>
    {
        public CreateAppointmentCommandValidator()
        {
            RuleFor(x => x.PlaceId)
                .GreaterThan(0)
                .WithMessage("PlaceId debe ser mayor a 0");

            RuleFor(x => x.AppointmentAt)
                .NotEmpty()
                .WithMessage("AppointmentAt es requerido")
                .Must(BeInFuture)
                .WithMessage("AppointmentAt debe ser una fecha futura");

            RuleFor(x => x.ServiceType)
                .NotEmpty()
                .WithMessage("ServiceType es requerido")
                .MaximumLength(200)
                .WithMessage("ServiceType no puede exceder 200 caracteres");

            RuleFor(x => x.Contact)
                .NotNull()
                .WithMessage("Contact es requerido");

            RuleFor(x => x.Contact.Name)
                .NotEmpty()
                .WithMessage("Contact.Name es requerido")
                .MaximumLength(200)
                .WithMessage("Contact.Name no puede exceder 200 caracteres");

            RuleFor(x => x.Contact.Email)
                .NotEmpty()
                .WithMessage("Contact.Email es requerido")
                .EmailAddress()
                .WithMessage("Contact.Email debe ser un email válido")
                .MaximumLength(200)
                .WithMessage("Contact.Email no puede exceder 200 caracteres");

            RuleFor(x => x.Contact.Phone)
                .MaximumLength(50)
                .When(x => x.Contact.Phone != null)
                .WithMessage("Contact.Phone no puede exceder 50 caracteres");

            RuleFor(x => x.Vehicle!.LicensePlate)
                .MaximumLength(20)
                .When(x => x.Vehicle != null && x.Vehicle.LicensePlate != null)
                .WithMessage("Vehicle.LicensePlate no puede exceder 20 caracteres");
        }

        private static bool BeInFuture(DateTime dateTime)
        {
            return dateTime > DateTime.UtcNow;
        }
    }
}
