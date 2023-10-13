using System.ComponentModel.DataAnnotations;

namespace server.Dtos
{
    public class TownNumberUpdateDto
    {
        [Required]
        public int TownNo { get; set; }
        [Required]
        public int TownId { get; set; }
        public string? SpecialDetail { get; set; }
    }
}