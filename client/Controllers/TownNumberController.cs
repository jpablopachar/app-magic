using AutoMapper;
using client.Dtos;
using client.Models;
using client.Services;
using client.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using utility;

namespace client.Controllers
{
    public class TownNumberController : Controller
    {
        private readonly ITownNumberService _townNumberService;
        private readonly ITownService _townService;
        private readonly IMapper _mapper;

        public TownNumberController(ITownNumberService townNumberService, ITownService townService, IMapper mapper)
        {
            _townNumberService = townNumberService;
            _townService = townService;
            _mapper = mapper;
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> IndexTownNumber()
        {
            List<TownNumberResponseDto> townsNumberList = new();

            var res = await _townNumberService.GetTownNumbers<ApiResponse>(HttpContext.Session.GetString(DS.SessionToken)!);

            if (res != null && res.Success == true) townsNumberList = JsonConvert.DeserializeObject<List<TownNumberResponseDto>>(Convert.ToString(res.Result)!)!;

            return View(townsNumberList);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CreateTownNumber()
        {
            TownNumberResponseVm townNumberResponseVm = new();

            var res = await _townService.GetTowns<ApiResponse>(HttpContext.Session.GetString(DS.SessionToken)!);

            if (res != null && res.Success == true) townNumberResponseVm.TownsList = JsonConvert.DeserializeObject<List<TownDto>>(Convert.ToString(res.Result)!)!.Select(tn => new SelectListItem { Text = tn.Name, Value = tn.Id.ToString() }).ToList();

            return View(townNumberResponseVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTownNumber(TownNumberResponseVm townNumberResponseVm)
        {
            if (ModelState.IsValid)
            {
                var result = await _townNumberService.CreateTownNumber<ApiResponse>(HttpContext.Session.GetString(DS.SessionToken)!, townNumberResponseVm.TownNumber!);

                if (result != null && result.Success)
                {
                    TempData["SuccessMessage"] = "Número de Villa creada correctamente.";

                    return RedirectToAction(nameof(IndexTownNumber));
                }
                else
                {
                    if (result!.ErrorMessages.Count > 0)
                    {
                        ModelState.AddModelError("ErrorMessages", result.ErrorMessages.FirstOrDefault()!);
                    }
                }
            }

            var res = await _townService.GetTowns<ApiResponse>(HttpContext.Session.GetString(DS.SessionToken)!);

            if (res != null && res.Success) townNumberResponseVm.TownsList = JsonConvert.DeserializeObject<List<TownDto>>(Convert.ToString(res.Result)!)!.Select(tn => new SelectListItem { Text = tn.Name, Value = tn.Id.ToString() });

            return View(townNumberResponseVm);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateTownNumber(int townNo)
        {
            TownNumberUpdateVm townNumberUpdateVm = new();

            var res = await _townNumberService.GetTownNumber<ApiResponse>(HttpContext.Session.GetString(DS.SessionToken)!, townNo);

            if (res != null && res.Success)
            {
                TownNumberResponseDto townNumberResponseDto = JsonConvert.DeserializeObject<TownNumberResponseDto>(Convert.ToString(res.Result)!)!;

                townNumberUpdateVm.TownNumber = _mapper.Map<TownNumberUpdateDto>(townNumberResponseDto);
            }

            res = await _townService.GetTowns<ApiResponse>(HttpContext.Session.GetString(DS.SessionToken)!);

            if (res != null && res.Success)
            {
                townNumberUpdateVm.TownsList = JsonConvert.DeserializeObject<List<TownDto>>(Convert.ToString(res.Result)!)!.Select(tn => new SelectListItem { Text = tn.Name, Value = tn.Id.ToString() });

                return View(townNumberUpdateVm);
            }

            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateTownNumber(TownNumberUpdateVm townNumberUpdateVm)
        {
            if (ModelState.IsValid)
            {
                var result = await _townNumberService.UpdateTownNumber<ApiResponse>(HttpContext.Session.GetString(DS.SessionToken)!, townNumberUpdateVm.TownNumber!);

                if (result != null && result.Success)
                {
                    TempData["SuccessMessage"] = "Número de Villa actualizada correctamente.";

                    return RedirectToAction(nameof(IndexTownNumber));
                }
                else
                {
                    if (result!.ErrorMessages.Count > 0)
                    {
                        ModelState.AddModelError("ErrorMessages", result.ErrorMessages.FirstOrDefault()!);
                    }
                }
            }

            var res = await _townService.GetTowns<ApiResponse>(HttpContext.Session.GetString(DS.SessionToken)!);

            if (res != null && res.Success) townNumberUpdateVm.TownsList = JsonConvert.DeserializeObject<List<TownDto>>(Convert.ToString(res.Result)!)!.Select(tn => new SelectListItem { Text = tn.Name, Value = tn.Id.ToString() });

            return View(townNumberUpdateVm);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> RemoveTownNumber(int townNo)
        {
            TownNumberDeleteVm townNumberDeleteVm = new();

            var res = await _townNumberService.GetTownNumber<ApiResponse>(HttpContext.Session.GetString(DS.SessionToken)!, townNo);

            if (res != null && res.Success)
            {
                TownNumberResponseDto townNumberResponseDto = JsonConvert.DeserializeObject<TownNumberResponseDto>(Convert.ToString(res.Result)!)!;

                townNumberDeleteVm.TownNumber = townNumberResponseDto;
            }

            res = await _townService.GetTowns<ApiResponse>(HttpContext.Session.GetString(DS.SessionToken)!);

            if (res != null && res.Success)
            {
                townNumberDeleteVm.TownsList = JsonConvert.DeserializeObject<List<TownDto>>(Convert.ToString(res.Result)!)!.Select(tn => new SelectListItem { Text = tn.Name, Value = tn.Id.ToString() });

                return View(townNumberDeleteVm);
            }

            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveTownNumber(TownNumberDeleteVm townNumberDeleteVm)
        {
            var res = await _townNumberService.RemoveTownNumber<ApiResponse>(HttpContext.Session.GetString(DS.SessionToken)!, townNumberDeleteVm.TownNumber!.TownNo);

            if (res != null && res.Success)
            {
                TempData["SuccessMessage"] = "Número de Villa eliminada correctamente.";

                return RedirectToAction(nameof(IndexTownNumber));
            }

            TempData["ErrorMessage"] = "Error al eliminar el número de Villa.";

            return View(townNumberDeleteVm);
        }
    }
}