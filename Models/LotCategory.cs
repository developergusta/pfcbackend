using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ticket2U.API.Models
{
    public class LotCategory
    {        
        [Key]
        public int LotCategoryId { get; set; }
        public string Desc { get; set; }
        public decimal PriceCategory { get; set; }
        public int? LotId { get; set; }
        [ForeignKey("LotId")]
        public Lot Lot { get; set; }
    }
}