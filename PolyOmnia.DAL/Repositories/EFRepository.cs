using Microsoft.EntityFrameworkCore;
using PolyOmnia.DAL.EF;
using PolyOmnia.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace PolyOmnia.DAL.Repositories
{
    public class EFRepository<T> : IRepository<T> where T : class
    {
        protected readonly PolyOmniaContext _context;

        public EFRepository(PolyOmniaContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<T>> GetAllAsync() =>
            await _context.Set<T>().ToListAsync();

        public async Task<T?> GetByIdAsync(int id) =>
            await _context.Set<T>().FindAsync(id);

        public async Task AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
        }

        public async Task UpdateAsync(T entity)
        {
            _context.Set<T>().Update(entity);
        }

        public async Task DeleteAsync(T entity)
        {
            _context.Set<T>().Remove(entity);
        }
    }
}
