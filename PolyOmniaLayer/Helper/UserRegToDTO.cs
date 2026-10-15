using PolyOmnia.BLL.DTO;
using PolyOmniaLayer.Models;
namespace PolyOmniaLayer.Helper
{
    public class UserRegToDTO
    {
        public static UserDTO ConvertToUserDTO(RegisterViewModel model)
        {
            UserDTO userDTO = new UserDTO();
            userDTO.Name = model.Name;
            userDTO.Surname = model.Surname;
            userDTO.Email = model.Email;
            userDTO.Login = model.Login;
            userDTO.PasswordHash = model.Password;

            return userDTO;
        }
    }
}
