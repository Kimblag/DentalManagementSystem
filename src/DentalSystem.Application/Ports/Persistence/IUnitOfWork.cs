namespace DentalSystem.Application.Ports.Persistence
{
    public interface IUnitOfWork
    {
        Task CommitAsync();
        bool HasChanges();
    }
}
