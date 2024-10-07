using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Tickets_Server.Models;

public partial class FeedBack
{
    [Key]
    public int FeedBackId { get; set; }

    public int? FeedBackType { get; set; }

    [StringLength(1000)]
    public string? Info { get; set; }

    [InverseProperty("FeedBack")]
    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
