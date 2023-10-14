using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using client.Models;
using client.Services;
using AutoMapper;
using client.Dtos;
using client.ViewModels;
using utility;
using Newtonsoft.Json;

namespace client.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ITownService _townService;
    private readonly IMapper _mapper;

    public HomeController(ILogger<HomeController> logger, ITownService townService, IMapper mapper)
    {
        _logger = logger;
        _townService = townService;
        _mapper = mapper;
    }

    public async Task<IActionResult> Index(int pageNumber = 1)
    {
        List<TownDto> townsList = new();
        TownPagedVm townPagedVm = new();

        if (pageNumber < 1) pageNumber = 1;

        var res = await _townService.GetTownsPaged<ApiResponse>(HttpContext.Session.GetString(DS.SessionToken)!, pageNumber, 4);

        if (res != null && res.Success)
        {
            townsList = JsonConvert.DeserializeObject<List<TownDto>>(Convert.ToString(res.Result)!)!;

            townPagedVm = new TownPagedVm
            {
                TownsList = townsList,
                PageNumber = pageNumber,
                TotalPages = JsonConvert.DeserializeObject<int>(Convert.ToString(res.TotalPages))
            };

            if (pageNumber > 1) townPagedVm.Prev = "";

            if (townPagedVm.TotalPages <= pageNumber) townPagedVm.Next = "disabled";
        }

        return View(townPagedVm);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
