using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using client.Dtos;
using client.Models;
using client.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using utility;

namespace client.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService) => _userService = userService;

        public IActionResult Login() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginRequestDto loginRequestDto)
        {
            var res = await _userService.Login<ApiResponse>(loginRequestDto);

            if (res != null && res.Success == true)
            {
                LoginResponseDto loginResponseDto = JsonConvert.DeserializeObject<LoginResponseDto>(Convert.ToString(res.Result)!)!;

                var handler = new JwtSecurityTokenHandler();
                var jwt = handler.ReadJwtToken(loginResponseDto.Token);

                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);

                identity.AddClaim(new(ClaimTypes.Name, jwt.Claims.FirstOrDefault(c => c.Type == "unique_name")?.Value!));

                identity.AddClaim(new(ClaimTypes.Role, jwt.Claims.FirstOrDefault(c => c.Type == "role")?.Value!));

                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                HttpContext.Session.SetString(DS.SessionToken, loginResponseDto.Token!);

                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("ErrorMessages", res!.ErrorMessages.FirstOrDefault()!);

                return View(loginRequestDto);
            }
        }

        public IActionResult Register() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterRequestDto registerRequestDto)
        {
            var res = await _userService.Register<ApiResponse>(registerRequestDto);

            if (res != null && res.Success) return RedirectToAction("Login");

            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();

            HttpContext.Session.SetString(DS.SessionToken, "");

            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied() => View();
    }
}