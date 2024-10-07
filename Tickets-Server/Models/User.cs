using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Tickets_Server.Models;

public partial class User
{
    [StringLength(100)]
    public string? Username { get; set; }

    [Key]
    [StringLength(100)]
    public string Password { get; set; } = null!;

    public int? Age { get; set; }

    [StringLength(100)]
    public string? Gender { get; set; }

    public int? RankId { get; set; }

    public int? FeedBackId { get; set; }

    [ForeignKey("FeedBackId")]
    [InverseProperty("Users")]
    public virtual FeedBack? FeedBack { get; set; }

    [ForeignKey("RankId")]
    [InverseProperty("Users")]
    public virtual Rank? Rank { get; set; }
}
