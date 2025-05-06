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

        public TeamDTO(Models.Team team)
        {
            this.TeamId = team.TeamId;
            this.Capacity = (int)team.Capacity;
            this.TeamName = team.TeamName;
            this.TeamCity = team.TeamCity;
            this.PriceForTicket = (int)team.PriceForTicket;
        }


    }
}
