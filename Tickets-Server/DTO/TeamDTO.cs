using System.ComponentModel.DataAnnotations;

namespace Tickets_Server.DTO
{
    public class TeamDTO
    {
        public int TeamId { get; set; }

        public int Capacity { get; set; }

        public string TeamName { get; set; }

        public string TeamCity { get; set; }

        public int PriceForTicket { get; set; }

        public TeamDTO() { }


    }
}
