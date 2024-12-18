using Microsoft.EntityFrameworkCore;
using Tickets_Server.Models;

namespace Tickets_Server.Models
{
    public partial class TicketsServerDBContext:DbContext
    {
        public User? GetUser(string email)
        {
           return this.Users.Where(x => x.Email == email).FirstOrDefault();
        }


    }
}
