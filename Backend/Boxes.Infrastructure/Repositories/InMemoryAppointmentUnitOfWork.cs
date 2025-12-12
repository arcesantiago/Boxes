using Boxes.Application.Contracts.Persistence;

namespace Boxes.Infrastructure.Repositories;

public class InMemoryAppointmentUnitOfWork : InMemoryUnitOfWorkBase, IAppointmentUnitOfWork
{
}