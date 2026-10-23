using AutoMapper;
using PolyOmnia.BLL.DTO;
using PolyOmnia.BLL.Interfaces;
using PolyOmnia.DAL.Entities;
using PolyOmnia.DAL.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace PolyOmnia.BLL.Services
{
    public class CategoryService(IUnitOfWork db, IMapper mapper) : ICategoryService
    {
        public async Task CreateCategory(CategoryDTO categoryDTO)
        {
            var category = mapper.Map<CategoryDTO, Category>(categoryDTO);
            await db.Category.AddAsync(category);
            await db.Save();
        }
        public async Task UpdateCategory(CategoryDTO categoryDTO)
        {
            var category = mapper.Map<CategoryDTO, Category>(categoryDTO);
            await db.Category.UpdateAsync(category);
            await db.Save();
        }
        public async Task DeleteCategory(CategoryDTO categoryDTO)
        {
            var category = await db.Category.GetByIdAsync(categoryDTO.Id);
            if (category == null)
            {
                throw new ValidationException($"Category with id {categoryDTO.Id} not found");
            }
            await db.Category.DeleteAsync(category);
            await db.Save();
        }
        public async Task<CategoryDTO?> GetCategoryById(int categoryId)
        {
            var category = await db.Category.GetByIdAsync(categoryId);
            if (category == null)
                throw new ValidationException($"Category with id {categoryId} not found");
            return mapper.Map<Category, CategoryDTO>(category);
        }
        public async Task<CategoryDTO?> GetCategoryByName(string name)
        {
            var category = await db.Category.GetByNameAsync(name);
            if (category == null)
                throw new ValidationException($"Category with name {name} not found");
            return mapper.Map<Category, CategoryDTO>(category);
        }
        public async Task<IEnumerable<CategoryDTO>> GetAllCategories()
        {
            var categories = await db.Category.GetAllAsync();
            return mapper.Map<IEnumerable<Category>, IEnumerable<CategoryDTO>>(categories);
        }
        public async Task<CategoryDTO?> GetCategoryWithVideos(int categoryId)
        {
            var categories = await db.Category.GetCategoryWithVideosAsync(categoryId);
            if (categories == null)
                throw new ValidationException($"Category with id {categoryId} not found");

            return mapper.Map<Category, CategoryDTO>(categories);
        }
        public async Task<IEnumerable<CategoryDTO>> GetAllWithVideoCount()
        {
            var categories = await db.Category.GetAllWithVideoCountAsync();
            return mapper.Map<IEnumerable<Category>, IEnumerable<CategoryDTO>>(categories);
        }
    }
}
