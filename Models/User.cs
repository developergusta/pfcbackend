using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ticket2U.API.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public String Name { get; set; }
        public DateTime? DateBirth { get; set; }
        public DateTime RegisterTime { get; set; }
        public string? Cpf { get; set; }
        public string? Rg { get; set; }
        public string Status { get; set; }
        public int? ImageId { get; set; }
        [ForeignKey("ImageId")]
        public Image Image { get; set; }
        public int? LoginId { get; set; }            
        [ForeignKey("LoginId")]
        public Login? Login { get; set; }
        /*public int? EventId { get; set; }
        [ForeignKey("EventId")]
        public Event Event { get; set; }*/
        public List<Event> Events { get; set; }
        public List<Phone> Phones { get; set; }
        public List<Address> Addresses { get; set; }
        public List<Ticket> Tickets { get; set; }
    }
}