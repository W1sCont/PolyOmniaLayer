using Microsoft.AspNetCore.Mvc.Rendering;
using PolyOmnia.BLL.DTO;

namespace PolyOmniaLayer.Models.Page
{
    public class FilterViewModel
    {
        public SelectList Categories { get; }
        public int? SelectedCategory { get; }
        public string? SelectedName { get; }

        public FilterViewModel(IEnumerable<CategoryDTO> categories, int? selectedCategory, string? selectedName)
        {
            Categories = new SelectList(categories, "Id", "Name", selectedCategory);
            SelectedCategory = selectedCategory;
            SelectedName = selectedName;
        }
    }
}