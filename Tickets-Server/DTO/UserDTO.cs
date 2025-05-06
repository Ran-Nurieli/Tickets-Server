using Tickets_Server.Models;

namespace Tickets_Server.DTO
{
    public class UserDTO
    {
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;

        public string Phone { get; set; } = null!;
        public string Email { get; set; } = null!;
        public int? Age { get; set; } = null!;
        public string Gender { get; set; } = null!;

        public bool IsAdmin {  get; set; }
        public string? HomeTeam { get; set; } = null!;
        //add TeamId

        public UserDTO() { }

        public UserDTO(Models.User user)
        {
            this.Username = user.Username;
            this.Password = user.Password;
            this.Phone = user.Phone;
            this.Email = user.Email;
            this.Age = ((int)user.Age);
            this.Gender = user.Gender;
            this.HomeTeam = user.FavoriteTeam?.TeamName;
            this.IsAdmin = user.IsAdmin;
        }


        public Models.User GetModels()
        {
            Models.User modelsUser = new Models.User()
            {
                Username = this.Username,
                Password = this.Password,
                Phone = this.Phone,
                Email = this.Email,
                Age = this.Age,
                Gender = this.Gender,
                IsAdmin = this.IsAdmin
            };
            return modelsUser;
        }
    }
}
