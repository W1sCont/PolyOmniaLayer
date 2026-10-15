using AutoMapper;
using PolyOmnia.BLL.DTO;
using PolyOmnia.DAL.Entities;

namespace PolyOmnia.BLL.Profiles
{
    public class PolyOmniaProfile : Profile
    {
        public PolyOmniaProfile()
        {
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<UserDTO, User>()
                .ForMember(dest => dest.Videos, opt => opt.Ignore());

            CreateMap<Video, VideoDTO>()
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : null))
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User != null ? src.User.Name : null));

            CreateMap<VideoDTO, Video>()
                .ForMember(dest => dest.Category, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore());

            CreateMap<Category, CategoryDTO>().ReverseMap();
        }
    }
}
