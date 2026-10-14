using Microsoft.EntityFrameworkCore;
using PolyOmnia.DAL.EF;
using PolyOmnia.DAL.Entities;
using PolyOmnia.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace PolyOmnia.DAL.Repositories
{
    public class CategoryRepository : EFRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(PolyOmniaContext context) : base(context) { }
        public async Task<Category?> GetByNameAsync(string name)
        {
            return await _context.Categories.AsNoTracking().FirstOrDefaultAsync(c => c.Name == name);
        }
        public async Task<bool> IsNameUniqueAsync(string name)
        {
            return !await _context.Categories.AsNoTracking().AnyAsync(c => c.Name == name);
        }
        public async Task<IEnumerable<Category>> GetCategoriesByIdsAsync(IEnumerable<int> categoryIds)
        {
            return await _context.Categories.AsNoTracking().Where(c => categoryIds.Contains(c.Id)).ToListAsync();
        }
        public async Task<Category?> GetCategoryWithVideosAsync(int categoryId)
        {
            return await _context.Categories
                .Include(c => c.Videos)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == categoryId);
        }
        public async Task<IEnumerable<Category>> GetAllWithVideoCountAsync()
        {
            return await _context.Categories
                .Include(c => c.Videos)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
