using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Tickets_Server.Models;

public partial class User
{
    [StringLength(100)]
    public string Username { get; set; } = null!;

    [StringLength(100)]
    public string Password { get; set; } = null!;

    [Key]
    [StringLength(100)]
    public string Email { get; set; } = null!;

    [StringLength(20)]
    public string Phone { get; set; } = null!;

    public int? Age { get; set; }

    [StringLength(100)]
    public string Gender { get; set; } = null!;

    public int? RankId { get; set; }

    public int? FeedBackId { get; set; }

    public bool IsAdmin { get; set; }

    public int? FavoriteTeamId { get; set; }

    [ForeignKey("FavoriteTeamId")]
    [InverseProperty("Users")]
    public virtual Team? FavoriteTeam { get; set; }

    [ForeignKey("FeedBackId")]
    [InverseProperty("Users")]
    public virtual FeedBack? FeedBack { get; set; }

    [InverseProperty("BuyerEmailNavigation")]
    public virtual ICollection<PurchaseRequest> PurchaseRequests { get; } = new List<PurchaseRequest>();

    [ForeignKey("RankId")]
    [InverseProperty("Users")]
    public virtual Rank? Rank { get; set; }

    [InverseProperty("UserEmailNavigation")]
    public virtual ICollection<Ticket> Tickets { get; } = new List<Ticket>();
}
