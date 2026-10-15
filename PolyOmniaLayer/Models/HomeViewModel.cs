using PolyOmnia.BLL.DTO;

namespace PolyOmniaLayer.Models
{
    public class HomeViewModel
    {
        public VideoDTO? HeroVideo { get; set; }
        public IEnumerable<VideoDTO> Videos { get; set; } = new List<VideoDTO>();
        public IEnumerable<CategoryDTO> Category { get; set; } = new List<CategoryDTO>();
        public int? SelectedCategoryId { get; set; }
    }
}
