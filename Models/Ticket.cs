using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ticket2U.API.Models
{
    public class Ticket
    {
        [Key]
        public int TicketId { get; set; }
        public DateTime RegisterTime { get; set; }
        public int? UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
        public int? EventId { get; set; }        
        [ForeignKey("EventId")]
        public Event Event { get; set; }
        public int? LotId { get; set; }       
        [ForeignKey("LotId")]
        public Lot Lot { get; set; } 
        public int? LotCategoryId { get; set; }
        [ForeignKey("LotCategoryId")]
        public LotCategory LotCategory { get; set; }
        public int? CashbackId { get; set; }
        [ForeignKey("CashbackId")]
        public Cashback Cashback { get; set; }
        
    }
}