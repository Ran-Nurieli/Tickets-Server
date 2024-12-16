using Microsoft.EntityFrameworkCore;
using Tickets_Server.Models;

namespace Tickets_Server.Models
{
    public partial class TicketServerDBContext:DbContext
    {
        public User? GetUser(string email)
        {
           return this.Users.where(x => x.Email == email).FirstOrDefault();
        }


    }
}
