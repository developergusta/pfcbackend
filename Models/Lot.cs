
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Ticket2U.API.Models
{
    public class Lot
    {
        [Key]
        public int LotId { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        [ForeignKey("EventId")]
        public int? EventId { get; set; }
        [JsonIgnore]
        public List<LotCategory> LotCategories { get; set; }
        public Event Event { get; set; }
        public List<Ticket> Tickets { get; set; }
    }
}
