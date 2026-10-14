using PolyOmnia.BLL.DTO;

namespace PolyOmnia.BLL.Interfaces
{
    public interface IVideoService
    {
        Task CreateVideo(VideoDTO videoDTO);
        Task UpdateVideo(VideoDTO videoDTO);
        Task DeleteVideo(VideoDTO videoDTO);
        Task<IEnumerable<VideoDTO>> GetApprovedVideos();
        Task<IEnumerable<VideoDTO>> GetAllWithDetails();
        Task<VideoDTO> GetByIdWithDetails(int id);
    }
}
