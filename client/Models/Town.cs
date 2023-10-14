using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace client.Models
{
    public class Town
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        public string? Detail { get; set; }
        [Required]
        public double Fee { get; set; }
        public int Occupants { get; set; }
        public int SquareMeter { get; set; }
        public string? ImageUrl { get; set; }
        public string? Amenity { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}