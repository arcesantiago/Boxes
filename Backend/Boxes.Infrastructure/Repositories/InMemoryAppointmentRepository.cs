using Boxes.Application.Contracts.Persistence;
using Boxes.Domain.Entities;

namespace Boxes.Infrastructure.Repositories
{
    public class InMemoryAppointmentRepository : InMemoryRepositoryBase<Appointment>, IAppointmentRepository
    {
    }
}
