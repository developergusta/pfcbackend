using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ticket2U.API.Models
{
    public class Event
    {
        [Key]
        public int EventId { get; set; }
        [Required]
        public String TitleEvent { get; set; }
        [Required]
        public String Category { get; set; }
        [Required]
        public int Capacity { get; set; }
        public string Description { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public string Status { get; set; }
        public int? UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }     
        public int? AddressId { get; set; }
        [ForeignKey("AddressId")]
        public Address Address { get; set; }
        public List<Lot> Lots { get; set; } 
        public List<Image> Images { get; set; }
        public List<Ticket> Tickets { get; set; }
    }
}