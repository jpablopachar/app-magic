using client.Dtos;

namespace client.ViewModels
{
    public class TownPagedVm
    {
        public int PageNumber { get; set; }
        public int TotalPages { get; set; }
        public string? Prev { get; set; } = "disabled";
        public string? Next { get; set; } = "";
        public IEnumerable<TownDto>? TownsList { get; set; }
    }
}