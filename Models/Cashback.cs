using System;
using System.ComponentModel.DataAnnotations;

namespace Ticket2U.API.Models
{
    public class Cashback
    {
        [Key]
        public int CashbackId { get; set; }
        public Ticket Ticket { get; set; }
        public string Description { get; set; }
        public DateTime DateSolicitation { get; set; }
        public DateTime? DateCashback { get; set; }
    }
}