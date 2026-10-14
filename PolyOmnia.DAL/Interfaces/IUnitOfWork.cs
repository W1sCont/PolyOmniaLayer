using PolyOmnia.DAL.Entities;

namespace PolyOmnia.DAL.Interfaces
{
    public interface IUnitOfWork
    {
        IUserRepository User { get; }
        IVideoRepository Video { get; }
        ICategoryRepository Category { get; }
        Task Save();
    }
}
