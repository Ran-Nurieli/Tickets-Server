using Tickets_Server.Models;
using Tickets_Server.DTO;
namespace Tickets_Server.ModelsHelpers
{
    public class TicketModelHelper
    {
        public static Ticket TicketDTOToTicket(TicketDTO ticketDTO)
        {
            return new Ticket()
            {
                Price = ticketDTO.Price,
                Place = ticketDTO.Place,
                Row = ticketDTO.Row,
                Seats = ticketDTO.Seats,
                TeamId = ticketDTO.TeamId,
            };

        }   
    }
}
