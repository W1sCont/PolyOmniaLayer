using PolyOmnia.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PolyOmnia.DAL.Interfaces
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<Category?> GetByNameAsync(string name);
        Task<bool> IsNameUniqueAsync(string name);

        Task<IEnumerable<Category>> GetCategoriesByIdsAsync(IEnumerable<int> categoryIds);
        Task<Category?> GetCategoryWithVideosAsync(int categoryId);

        Task<IEnumerable<Category>> GetAllWithVideoCountAsync();
    }
}
