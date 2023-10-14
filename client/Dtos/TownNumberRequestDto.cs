using System.ComponentModel.DataAnnotations;

namespace client.Dtos
{
    public class TownNumberRequestDto
    {
        [Required]
        public int TownNo { get; set; }
        [Required]
        public int TownId { get; set; }
        public string? SpecialDetail { get; set; }
    }
}