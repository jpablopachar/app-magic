using client.Dtos;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace client.ViewModels
{
    public class TownNumberUpdateVm
    {
        public TownNumberUpdateDto TownNumber { get; set; }
        public IEnumerable<SelectListItem>? TownsList { get; set; }

        public TownNumberUpdateVm()
        {
            TownNumber = new TownNumberUpdateDto();
        }
    }
}