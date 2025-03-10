using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Tickets_Server.Models;

public partial class Ticket
{
    [Key]
    public int TicketId { get; set; }

    public int? Price { get; set; }

    [StringLength(100)]
    public string Place { get; set; } = null!;

    public int? Row { get; set; }

    public int? Seats { get; set; }

    public int? TeamId { get; set; }

    [ForeignKey("TeamId")]
    [InverseProperty("Tickets")]
    public virtual Team? Team { get; set; }

    public Ticket(Models.Ticket ticket)
    {
        this.TicketId++;
        this.Price = ticket.Price;
        this.Place = ticket.Place;
        this.Row = ticket.Row;
        this.Seats = ticket.Seats;
        this.TeamId = ticket.TeamId;
        this.TeamId = 0;
    }
}
