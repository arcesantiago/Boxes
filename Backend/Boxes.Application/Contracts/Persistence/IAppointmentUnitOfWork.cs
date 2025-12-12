namespace Boxes.Application.Contracts.Persistence
{
    public interface IAppointmentUnitOfWork : IDisposable
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}