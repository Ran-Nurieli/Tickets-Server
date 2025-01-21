namespace Tickets_Server.DTO
{
    public class UserDTO
    {
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;

        public string Email { get; set; } = null!;
        public int? Age { get; set; } = null!;
        public string Gender { get; set; } = null!;

        public bool IsAdmin {  get; set; }

      
        public UserDTO() { }

        public UserDTO(Models.User user)
        {
            this.Username = user.Username;
            this.Password = user.Password;
            this.Email = user.Email;
            this.Age = ((int)user.Age);
            this.Gender = user.Gender;
            this.IsAdmin = user.IsAdmin;
        }


        public Models.User GetModels()
        {
            Models.User modelsUser = new Models.User()
            {
                Username = this.Username,
                Password = this.Password,
                Email = this.Email,
                Age = this.Age,
                Gender = this.Gender,
                IsAdmin = this.IsAdmin
            };
            return modelsUser;
        }
    }
}
