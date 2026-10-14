using AutoMapper;
using PolyOmnia.BLL.DTO;
using PolyOmnia.DAL.Entities;

namespace PolyOmnia.BLL.Profiles
{
    public class PolyOmniaProfile : Profile
    {
        public PolyOmniaProfile()
        {
            CreateMap<User, UserDTO>();

            CreateMap<Video, VideoDTO>()
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User.Name));

            CreateMap<Category, CategoryDTO>();
        }
    }
}
