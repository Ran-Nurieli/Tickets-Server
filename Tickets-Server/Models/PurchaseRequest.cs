using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Tickets_Server.Models;

public partial class PurchaseRequest
{
    [Key]
    public int TicketId { get; set; }

    [StringLength(100)]
    public string BuyerEmail { get; set; } = null!;

    public bool IsAccepted { get; set; }

    [ForeignKey("BuyerEmail")]
    [InverseProperty("PurchaseRequests")]
    public virtual User BuyerEmailNavigation { get; set; } = null!;

    [ForeignKey("TicketId")]
    [InverseProperty("PurchaseRequest")]
    public virtual Ticket Ticket { get; set; } = null!;
}
