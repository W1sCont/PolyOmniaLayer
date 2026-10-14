using PolyOmnia.BLL.DTO;

namespace PolyOmnia.BLL.Interfaces
{
    public interface ICategoryService
    {
        Task CreateCategory(CategoryDTO categoryDTO);
        Task UpdateCategory(CategoryDTO categoryDTO);
        Task DeleteCategory(CategoryDTO categoryDTO);
        Task<CategoryDTO?> GetCategoryById(int categoryId);
        Task<CategoryDTO?> GetCategoryByName(string name);
        Task<IEnumerable<CategoryDTO>> GetAllCategories();
        Task<CategoryDTO?> GetCategoryWithVideos(int categoryId);
        Task<IEnumerable<CategoryDTO>> GetAllWithVideoCount();
    }
}
