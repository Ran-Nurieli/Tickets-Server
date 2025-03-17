using System.ComponentModel.DataAnnotations;

namespace Tickets_Server.DTO
{
    public class TicketDTO
    {
        public int TicketId { get; set; }

        public int Price { get; set; }

        public string Place { get; set; }

        public int Row { get; set; }

        public int Seats { get; set; }

        public int TeamId { get; set; }

        public TicketDTO() { }


       


    }
}
