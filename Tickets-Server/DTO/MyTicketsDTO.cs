using Tickets_Server.Models;
namespace Tickets_Server.DTO
{
    public class MyTicketsDTO
    {
        public int TicketId { get; set; }
        public string Username { get; set; } = null!;
        public string? TeamName { get; set; } = null!;
        public int Gate { get; set; }
        public int? Row { get; set; } 
        public int? Seats { get; set; } = null!;
        public DateTime Date { get; set; }
        public int Price { get; set; }
        public bool IsAccepted { get; set; } = false;
        public string? BuyerUsername { get; set; } = null!;
        public string? BuyerPhone { get; set; } = null!;
        public string AwayTeam { get; set; } = null!;
        public string HomeTeam { get; set; } = null!;
        public MyTicketsDTO() { }

        public MyTicketsDTO(Ticket ticket)
        {
            this.TicketId = ticket.TicketId;
            this.TeamName = ticket.Team?.TeamName;
            this.Gate = ticket.Gate;
            this.Row = ticket.Row;
            this.Seats = ticket.Seats;
            this.Price = ticket.Price ?? 0;
            this.BuyerUsername = new string(ticket.PurchaseRequest?.BuyerEmailNavigation?.Username.ToCharArray());
            this.BuyerPhone = new string(ticket.PurchaseRequest?.BuyerEmailNavigation?.Phone.ToCharArray());
            this.AwayTeam = ticket.Team?.TeamName + ticket.Team?.TeamCity;
            this.IsAccepted = ticket.PurchaseRequest?.IsAccepted ?? false;

        }   
    }
}
