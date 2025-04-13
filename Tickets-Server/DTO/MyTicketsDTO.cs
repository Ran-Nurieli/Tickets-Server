using Tickets_Server.Models;
namespace Tickets_Server.DTO
{
    public class MyTicketsDTO
    {
        public int TicketId { get; set; }
        public string Username { get; set; } = null!;
        public string? TeamName { get; set; } = null!;
        public string Place { get; set; } = null!;
        public int? Row { get; set; } 
        public int? Seats { get; set; } = null!;
        public DateTime Date { get; set; }
        public int Price { get; set; }
        public bool IsAccepted { get; set; } = false;
        public string? BuyerUsername { get; set; } = null!;
        public string? BuyerPhone { get; set; } = null!;
        public MyTicketsDTO() { }

        public MyTicketsDTO(Ticket ticket)
        {
            this.TicketId = ticket.TicketId;
            this.TeamName = ticket.Team?.TeamName;
            this.Place = ticket.Place;
            this.Row = ticket.Row;
            this.Seats = ticket.Seats;
            this.Price = ticket.Price ?? 0;
            this.BuyerUsername = ticket.PurchaseRequest?.BuyerEmailNavigation?.Username;
            this.BuyerPhone = ticket.PurchaseRequest?.BuyerEmailNavigation?.Phone;
            this.IsAccepted = ticket.PurchaseRequest?.IsAccepted ?? false;

        }   
    }
}
