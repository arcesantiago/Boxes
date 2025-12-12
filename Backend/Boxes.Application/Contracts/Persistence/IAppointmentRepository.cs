using Boxes.Application.Contracts.Persistence.Read;
using Boxes.Application.Contracts.Persistence.Write;
using Boxes.Domain.Entities;

namespace Boxes.Application.Contracts.Persistence
{
    public interface IAppointmentRepository : IReadRepository<Appointment>, IWriteRepository<Appointment>
    {
    }
}
