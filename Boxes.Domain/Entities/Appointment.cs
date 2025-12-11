using Boxes.Domain.Common;

namespace Boxes.Domain.Entities
{
    public class Appointment : BaseDomainModel
    {
        public int PlaceId { get; private set; }
        public DateTime AppointmentAt { get; private set; }
        public string ServiceType { get; private set; } = string.Empty;
        public Contact Contact { get; private set; } = null!;
        public Vehicle? Vehicle { get; private set; }

        private Appointment() { }

        public Appointment(
            int placeId,
            DateTime appointmentAt,
            string serviceType,
            Contact contact,
            Vehicle? vehicle = null)
        {

            if (placeId <= 0)
                throw new ArgumentException("PlaceId must be greater than 0.", nameof(placeId));

            if (appointmentAt <= DateTime.UtcNow)
                throw new ArgumentException("Appointment date must be in the future.", nameof(appointmentAt));

            if (string.IsNullOrWhiteSpace(serviceType))
                throw new ArgumentException("Service type is required.", nameof(serviceType));

            Contact = contact ?? throw new ArgumentNullException(nameof(contact));

            PlaceId = placeId;
            AppointmentAt = appointmentAt;
            ServiceType = serviceType;
            Contact = contact;
            Vehicle = vehicle;
            CreatedAt = DateTimeOffset.UtcNow;
            UpdatedAt = DateTimeOffset.UtcNow;
        }
    }
}
