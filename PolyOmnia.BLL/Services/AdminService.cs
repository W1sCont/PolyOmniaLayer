using Microsoft.EntityFrameworkCore;
using PolyOmnia.BLL.DTO;
using PolyOmnia.DAL.Enums;

namespace PolyOmnia.BLL.Services
{
    public class AdminService
    {
        public async Task<IEnumerable<UserDTO>> GetPendingUsersAsync()
        {
            return await _context.Users.AsNoTracking().Where(u => u.Role == UserRole.Unconfirmed).ToListAsync();
        }

        public async Task ApproveUserAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user != null)
            {
                user.Role = UserRole.User;
                await _context.SaveChangesAsync();
            }
        }

        public async Task RejectUserAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Video>> GetPendingVideosAsync()
        {
            return await _context.Videos.AsNoTracking()
                .Include(v => v.Genre)
                .Include(v => v.User)
                .Where(v => !v.IsApproved)
                .OrderByDescending(v => v.CreatedAt)
                .ToListAsync();
        }

        public async Task ApproveVideoAsync(int videoId)
        {
            var video = await _context.Videos.FindAsync(videoId);
            if (video != null)
            {
                video.IsApproved = true;
                await _context.SaveChangesAsync();
            }
        }

        public async Task RejectVideoAsync(int videoId)
        {
            var video = await _context.Videos.FindAsync(videoId);
            if (video != null)
            {
                _context.Videos.Remove(video);
                await _context.SaveChangesAsync();
            }
        }
    }
}
