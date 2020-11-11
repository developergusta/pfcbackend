using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ticket2U.API.Models
{
    public class Phone
    {        
        [Key]
        public int PhoneId { get; set; }
        public int? Type { get; set; }
        public string Number { get; set; }
        [ForeignKey("UserId")]
        public User User { get; }
        public int? UserId { get; set; }
    }
}