using AutoMapper;
using PolyOmnia.BLL.DTO;
using PolyOmnia.BLL.Interfaces;
using PolyOmnia.DAL.Entities;
using PolyOmnia.DAL.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace PolyOmnia.BLL.Services
{
    public class VideoService(IUnitOfWork db, IMapper mapper) : IVideoService
    {
        public async Task CreateVideo(VideoDTO videoDTO)
        {
            var video = mapper.Map<VideoDTO, Video>(videoDTO);
            await db.Video.AddAsync(video);
            await db.Save();
        }

        public async Task UpdateVideo(VideoDTO videoDTO)
        { 
            var existingVideo = db.Video.GetByIdWithDetailsAsync(videoDTO.Id);
            if (existingVideo == null)
                throw new ValidationException($"Video with id {videoDTO.Id} not found");

            var video = mapper.Map<VideoDTO, Video>(videoDTO);
            await db.Save();
        }

        public async Task DeleteVideo(VideoDTO videoDTO)
        {
            var video = mapper.Map<VideoDTO, Video>(videoDTO);
            if(video == null)
            {
                throw new ValidationException($"Video with id {videoDTO.Id} not found");
            }
            await db.Video.DeleteAsync(video);
            await db.Save();
        }

        public async Task<IEnumerable<VideoDTO>> GetApprovedVideos()
        {
            var videos = await db.Video.GetApprovedVideosAsync();
            return mapper.Map<IEnumerable<Video>, IEnumerable<VideoDTO>>(videos);
        }

        public async Task<IEnumerable<VideoDTO>> GetAllWithDetails()
        {
            var videos = await db.Video.GetAllWithDetailsAsync();
            return mapper.Map<IEnumerable<Video>, IEnumerable<VideoDTO>>(videos);
        }

        public async Task<VideoDTO> GetByIdWithDetails(int id)
        {
            var video = await db.Video.GetByIdWithDetailsAsync(id);
            if(video == null)
            {
                throw new ValidationException($"Video with id {id} not found");
            }
            return mapper.Map<Video, VideoDTO>(video);
        }
    }
}
