namespace Tickets_Server.DTO
{
    public class TicketReadDTO
    {

        public int TicketId { get; set; }

        public int Price { get; set; }

        public int Gate { get; set; }

        public int Row { get; set; }

        public int Seats { get; set; }

        public string HomeTeam { get; set; }
        public string AwayTeam { get; set; }

        public TicketReadDTO(Models.Ticket ticket)
        {
            this.TicketId = ticket.TicketId;
            this.Price = (int)ticket.Price;
            this.Gate = (int)ticket.Gate;
            this.Row = (int)ticket.Row;
            this.Seats = (int)ticket.Seats;
            this.HomeTeam = ticket.Team?.TeamName + ticket.Team?.TeamCity;
            this.AwayTeam = ticket.AwayTeam?.TeamName + ticket.AwayTeam?.TeamCity;

        }
    }
}
