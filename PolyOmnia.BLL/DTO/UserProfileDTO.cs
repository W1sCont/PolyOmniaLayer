using System;
using System.Collections.Generic;
using System.Text;

namespace PolyOmnia.BLL.DTO
{
    public class UserProfileDTO
    {
        public int UserId { get; set; } = 0;
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime RegisteredAt { get; set; }
        public IEnumerable<VideoDTO> UserVideos { get; set; } = Enumerable.Empty<VideoDTO>();
        public long TotalViewsCount => UserVideos?.Sum(v => v.ViewCount) ?? 0;
    }
}
