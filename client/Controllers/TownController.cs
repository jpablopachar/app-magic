using AutoMapper;
using client.Dtos;
using client.Models;
using client.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using utility;

namespace client.Controllers
{
    public class TownController : Controller
    {
        private readonly ITownService _townService;
        private readonly IMapper _mapper;

        public TownController(ITownService townService, IMapper mapper)
        {
            _townService = townService;
            _mapper = mapper;
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> IndexTown()
        {

            List<TownDto> townsList = new();

            var res = await _townService.GetTowns<ApiResponse>(HttpContext.Session.GetString(DS.SessionToken)!);

            if (res != null && res.Success == true) townsList = JsonConvert.DeserializeObject<List<TownDto>>(Convert.ToString(res.Result)!)!;

            return View(townsList);
        }

        [Authorize(Roles = "admin")]
        public IActionResult CreateTown() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTown(TownRequestDto townRequestDto)
        {
            if (ModelState.IsValid)
            {
                var res = await _townService.CreateTown<ApiResponse>(HttpContext.Session.GetString(DS.SessionToken)!, townRequestDto);

                if (res != null && res.Success)
                {
                    TempData["SuccessMessage"] = "Villa creada correctamente.";

                    return RedirectToAction(nameof(IndexTown));
                }
            }

            return View(townRequestDto);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateTown(int townId)
        {
            var res = await _townService.GetTown<ApiResponse>(HttpContext.Session.GetString(DS.SessionToken)!, townId);

            if (res != null && res.Success)
            {
                TownDto townDto = JsonConvert.DeserializeObject<TownDto>(Convert.ToString(res.Result)!)!;

                return View(_mapper.Map<TownUpdateDto>(townDto));
            }

            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateTown(TownUpdateDto townUpdateDto)
        {
            if (ModelState.IsValid)
            {
                var res = await _townService.UpdateTown<ApiResponse>(HttpContext.Session.GetString(DS.SessionToken)!, townUpdateDto);

                if (res != null && res.Success)
                {
                    TempData["SuccessMessage"] = "Villa actualizada correctamente.";

                    return RedirectToAction(nameof(IndexTown));
                }
            }

            return View(townUpdateDto);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> RemoveTown(int townId)
        {
            var res = await _townService.GetTown<ApiResponse>(HttpContext.Session.GetString(DS.SessionToken)!, townId);

            if (res != null && res.Success)
            {
                TownDto townDto = JsonConvert.DeserializeObject<TownDto>(Convert.ToString(res.Result)!)!;

                return View(townDto);
            }

            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveTown(TownDto townDto)
        {
            var res = await _townService.RemoveTown<ApiResponse>(HttpContext.Session.GetString(DS.SessionToken)!, townDto.Id);

            if (res != null && res.Success)
            {
                TempData["SuccessMessage"] = "Villa eliminada correctamente.";

                return RedirectToAction(nameof(IndexTown));
            }

            TempData["ErrorMessage"] = "No se ha podido eliminar la villa.";

            return View(townDto);
        }
    }
}