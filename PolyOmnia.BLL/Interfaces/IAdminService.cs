using PolyOmnia.BLL.DTO;

namespace PolyOmnia.BLL.Interfaces
{
    internal interface IAdminService
    {
        Task<IEnumerable<UserDTO>> GetPendingUsersAsync();
        Task ApproveUserAsync(int userId);
        Task RejectUserAsync(int userId);
        Task<IEnumerable<VideoDTO>> GetPendingVideosAsync();
        Task ApproveVideoAsync(int videoId);
        Task RejectVideoAsync(int videoId);
    }
}
