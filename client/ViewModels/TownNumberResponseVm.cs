using client.Dtos;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace client.ViewModels
{
    public class TownNumberResponseVm
    {
        public TownNumberRequestDto? TownNumber { get; set; }
        public IEnumerable<SelectListItem>? TownsList { get; set; }

        public TownNumberResponseVm()
        {
            TownNumber = new TownNumberRequestDto();
        }
    }
}