namespace Tickets_Server.DTO
{
    public class UserDTO
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public string Email { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }

      
        public UserDTO(Models.User user)
        {
            this.Username = Username;
            this.Password = Password;
            this.Age = Age;
            this.Gender = Gender;
        }


        public Models.User GetModels()
        {
            Models.User modelsUser = new Models.User()
            {
                Username = this.Username,
                Password = this.Password,
                Age = this.Age,
                Gender = this.Gender
            };
            return modelsUser;
        }
    }
}
