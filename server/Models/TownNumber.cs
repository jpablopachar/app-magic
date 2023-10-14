using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace server.Models
{
    public class TownNumber
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int TownNo { get; set; }
        [Required]
        public int TownId { get; set; }
        [ForeignKey("TownId")]
        public Town? Town { get; set; }
        public string? SpecialDetail { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}