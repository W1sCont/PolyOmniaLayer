using PolyOmnia.DAL.Entities;

namespace PolyOmnia.DAL.Interfaces
{
    public interface IVideoRepository : IRepository<Video>
    {
        Task<IEnumerable<Video>> GetApprovedVideosAsync();
        Task<IEnumerable<Video>> GetAllWithDetailsAsync();
        Task<Video?> GetByIdWithDetailsAsync(int id);
        Task<IEnumerable<Video>> GetVideoListByUserIdAsync(int id);
    }
}
