using System.ComponentModel.DataAnnotations;

namespace PolyOmniaLayer.Models
{
    public class CategoryViewModel
    {
        public class CreateCategoryViewModel
        {
            [Required(ErrorMessage = "Вкажіть назву жанру/категорії")]
            [StringLength(50, ErrorMessage = "Назва не повинна перевищувати 50 символів")]
            public string Name { get; set; } = string.Empty;
        }

        public class EditCategoryViewModel
        {
            public int Id { get; set; }

            [Required(ErrorMessage = "Вкажіть назву жанру/категорії")]
            [StringLength(50, ErrorMessage = "Назва не повинна перевищувати 50 символів")]
            public string Name { get; set; } = string.Empty;
        }
    }
}
