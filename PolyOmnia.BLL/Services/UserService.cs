using AutoMapper;
using PolyOmnia.BLL.DTO;
using PolyOmnia.BLL.Interfaces;
using PolyOmnia.DAL.Entities;
using PolyOmnia.DAL.Interfaces;
using System.ComponentModel.DataAnnotations;
using PolyOmnia.DAL.Enums;

namespace PolyOmnia.BLL.Services
{
    public class UserService(IUnitOfWork db, IMapper mapper) : IUserService
    {
        public async Task CreateUser(UserDTO model)
        {
            var user = mapper.Map<UserDTO, User>(model);
            await db.User.AddAsync(user);
        }

        public async Task UpdateUser(UserDTO model)
        {
            var user = mapper.Map<UserDTO, User>(model);
            await db.User.UpdateAsync(user);
        }

        public async Task DeleteUser(int userId)
        {
            var user = await db.User.GetByIdAsync(userId);
            if (user == null)
            {
                throw new ValidationException($"User with id {userId} not found");
            }
            await db.User.DeleteAsync(user);
        }

        public async Task<UserDTO?> GetUserById(int userId)
        {
            var user = await db.User.GetByIdAsync(userId);
            if (user == null)
                throw new ValidationException($"User with id {userId} not found");

            return mapper.Map<User, UserDTO>(user);
        }

        public async Task<UserDTO?> GetUserByEmail(string email)
        {
            var user = await db.User.GetByEmailAsync(email);
            if (user == null)
                throw new ValidationException($"User with email {email} not found");

            return mapper.Map<User, UserDTO>(user);
        }

        public async Task<UserDTO?> GetUserByLogin(string login)
        {
            var user = await db.User.GetByLoginAsync(login);
            if (user == null)
                throw new ValidationException($"User with login {login} not found");

            return mapper.Map<User, UserDTO>(user);
        }

        public async Task<UserDTO?> GetUserByLoginOrEmail(string emailOrLogin)
        {
            var user = await db.User.GetByLoginOrEmailAsync(emailOrLogin);
            if (user == null)
                throw new ValidationException($"User with login or email {emailOrLogin} not found");

            return mapper.Map<User, UserDTO>(user);
        }

        public async Task<bool> IsEmailUnique(string email)
        {
            var user = await db.User.GetByEmailAsync(email);
            return user == null;
        }

        public async Task<bool> IsLoginUnique(string login)
        {
            var user = await db.User.GetByLoginAsync(login);
            return user == null;
        }

        public async Task UpdateUserRole(int userId, string newRole)
        {
            var user = await db.User.GetByIdAsync(userId);
            if (user == null)
                throw new ValidationException($"User with id {userId} not found");
            
            if (!Enum.TryParse<UserRole>(newRole, true, out var parsedRole))
                throw new ValidationException($"Invalid role: {newRole}");
            
            user.Role = parsedRole;
            await db.User.UpdateAsync(user);
        }

        public async Task<bool> IsUserUnconfirmed(int id)
        {
            var user = await db.User.GetByIdAsync(id);
            if (user == null)
                throw new ValidationException($"User with id {id} not found");
            
            return user.Role == UserRole.Unconfirmed;
        }

        public async Task<IEnumerable<UserDTO>> GetUsersByRole(string role)
        {
            if (!Enum.TryParse<UserRole>(role, true, out var parsedRole))
                throw new ValidationException($"Invalid role: {role}");

            var users = await db.User.GetUsersByRoleAsync(parsedRole);
            return mapper.Map<IEnumerable<User>, IEnumerable<UserDTO>>(users);
        }

        public async Task<UserDTO?> GetUserWithVideos(int userId)
        {
            var user = await db.User.GetUserWithVideosAsync(userId);
            if (user == null)
                throw new ValidationException($"User with id {userId} not found");

            return mapper.Map<User, UserDTO>(user);
        }
    }
}
