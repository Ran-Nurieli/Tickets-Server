using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Tickets_Server.Models;

public partial class Rank
{
    [Key]
    public int RankId { get; set; }

    [StringLength(100)]
    public string? RankType { get; set; }

    [InverseProperty("Rank")]
    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
