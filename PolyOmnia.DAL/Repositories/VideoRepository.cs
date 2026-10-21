using Microsoft.EntityFrameworkCore;
using PolyOmnia.DAL.EF;
using PolyOmnia.DAL.Entities;
using PolyOmnia.DAL.Interfaces;


namespace PolyOmnia.DAL.Repositories
{
    public class VideoRepository : EFRepository<Video>, IVideoRepository
    {
        public VideoRepository(PolyOmniaContext context) : base(context) { }

        public async Task<IEnumerable<Video>> GetApprovedVideosAsync()
        {
            return await _context.Videos.AsNoTracking()
                .Include(v => v.Category)
                .Include(v => v.User)
                .Where(v => v.IsApproved && v.IsPublished)
                .OrderByDescending(v => v.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Video>> GetAllWithDetailsAsync()
        {
            return await _context.Videos.AsNoTracking()
                .Include(v => v.Category)
                .Include(v => v.User)
                .OrderByDescending(v => v.CreatedAt)
                .ToListAsync();
        }

        public async Task<Video?> GetByIdWithDetailsAsync(int id)
        {
            return await _context.Videos
                .Include(v => v.User)
                .Include(v => v.Category)
                .FirstOrDefaultAsync(v => v.Id == id);
        }

        public async Task<IEnumerable<Video>> GetVideoListByUserIdAsync(int id)
        {
            return await _context.Videos.AsNoTracking()
                .Include(v => v.User)
                .Include(v => v.Category)
                .Where(v => v.UserId == id)
                .ToListAsync();
        }
    }
}
