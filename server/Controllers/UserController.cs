using System.Net;
using Microsoft.AspNetCore.Mvc;
using server.Repositories;
using server.Models;
using server.Dtos;

namespace server.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    // [ApiVersionNeutral]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly ApiResponse _apiResponse;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _apiResponse = new();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            var loginResponse = await _userRepository.Login(loginRequestDto);

            if (loginResponse.User == null || string.IsNullOrEmpty(loginResponse.Token))
            {
                _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                _apiResponse.Success = false;
                _apiResponse.ErrorMessages.Add("Username o password incorrectos");

                return BadRequest(_apiResponse);
            }

            _apiResponse.Success = true;
            _apiResponse.StatusCode = HttpStatusCode.OK;
            _apiResponse.Result = loginResponse;

            return Ok(_apiResponse);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto)
        {
            bool isUserUnique = _userRepository.IsUserUnique(registerRequestDto.Username!);

            if (!isUserUnique)
            {
                _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                _apiResponse.Success = false;
                _apiResponse.ErrorMessages.Add("El usuario ya existe");

                return BadRequest(_apiResponse);
            }

            var user = await _userRepository.Register(registerRequestDto);

            if (user == null)
            {
                _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                _apiResponse.Success = false;
                _apiResponse.ErrorMessages.Add("No se pudo registrar el usuario");

                return BadRequest(_apiResponse);
            }

            _apiResponse.Success = true;
            _apiResponse.StatusCode = HttpStatusCode.OK;

            return Ok(_apiResponse);
        }
    }
}