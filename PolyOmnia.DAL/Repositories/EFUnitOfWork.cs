using PolyOmnia.DAL.EF;
using PolyOmnia.DAL.Entities;
using PolyOmnia.DAL.Interfaces;

namespace PolyOmnia.DAL.Repositories
{
    public class EFUnitOfWork(PolyOmniaContext context) : IUnitOfWork
    {
        private UserRepository? _userRepository;
        private VideoRepository? _videoRepository;
        private CategoryRepository? _categoryRepository;
        public IUserRepository User => _userRepository ??= new UserRepository(context);
        public IVideoRepository Video => _videoRepository ??= new VideoRepository(context);
        public ICategoryRepository Category => _categoryRepository ??= new CategoryRepository(context);
        public async Task Save() => await context.SaveChangesAsync();
    }
}
