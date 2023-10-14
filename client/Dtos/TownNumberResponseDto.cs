namespace client.Dtos
{
    public class TownNumberResponseDto
    {
        public int TownNo { get; set; }
        public int TownId { get; set; }
        public string? SpecialDetail { get; set; }
        public TownDto? Town { get; set; }
    }
}