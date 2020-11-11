using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ticket2U.API.Models
{
    public class Image
    {
        [Key]
        public int IdImage { get; set; }
        public string Alt { get; set; }
        [Required]
        public string Src { get; set; }
        public User User { get; set; }
        [ForeignKey("EventId")]
        public Event Event { get; set; }
        public int? EventId { get; set; }
    }
}