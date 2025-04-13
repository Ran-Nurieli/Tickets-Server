using Tickets_Server.DTO;
using Tickets_Server.Models;
namespace Tickets_Server.ModelsHelpers
{
    public class UserModelHelper
    {

        public static User UserDTOToUser(UserDTO userDTO)
        {
            return new User()
            {
                Email = userDTO.Email,
                Password = userDTO.Password,
                Username = userDTO.Username,
                Phone = userDTO.Phone,
                Age = userDTO.Age,
                Gender = userDTO.Gender,
                IsAdmin = userDTO.IsAdmin,
            };
        }
    }
}
