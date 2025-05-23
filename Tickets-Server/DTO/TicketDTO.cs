﻿using System.ComponentModel.DataAnnotations;

namespace Tickets_Server.DTO
{
    public class TicketDTO
    {
        public int TicketId { get; set; }

        public int Price { get; set; }

        public int Gate { get; set; }

        public int Row { get; set; }

        public int Seats { get; set; }

        public int TeamId { get; set; }
        public int AwayTeamId { get; set; }
        public TicketDTO() { }

        public TicketDTO(Models.Ticket ticket)
        {
            this.TicketId = ticket.TicketId;
            this.Price = (int)ticket.Price;
            this.Gate = (int)ticket.Gate;
            this.Row = (int)ticket.Row;
            this.Seats = (int)ticket.Seats;
            this.TeamId = (int)ticket.TeamId;
            this.AwayTeamId = (int)ticket.AwayTeamId;

        }
       


    }
}
