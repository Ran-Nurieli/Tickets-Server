using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Tickets_Server.Models;

public partial class Team
{
    [Key]
    public int TeamId { get; set; }

    public int? Capacity { get; set; }

    [StringLength(100)]
    public string TeamName { get; set; } = null!;

    [StringLength(100)]
    public string TeamCity { get; set; } = null!;

    public int? PriceForTicket { get; set; }

    [InverseProperty("AwayTeam")]
    public virtual ICollection<Ticket> TicketAwayTeams { get; } = new List<Ticket>();

    [InverseProperty("Team")]
    public virtual ICollection<Ticket> TicketTeams { get; } = new List<Ticket>();

    [InverseProperty("FavoriteTeam")]
    public virtual ICollection<User> Users { get; } = new List<User>();
}
