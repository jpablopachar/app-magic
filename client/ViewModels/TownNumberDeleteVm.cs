using client.Dtos;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace client.ViewModels
{
    public class TownNumberDeleteVm
    {
        public TownNumberResponseDto TownNumber { get; set; }
        public IEnumerable<SelectListItem>? TownsList { get; set; }

        public TownNumberDeleteVm()
        {
            TownNumber = new TownNumberResponseDto();
        }
    }
}