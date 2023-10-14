using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using server.Dtos;
using server.Models;
using server.Repositories;
using server.Specifications;

namespace server.Controllers.v1
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class TownController : ControllerBase
    {
        private readonly ILogger<TownController> _logger;
        private readonly ITownRepository _townRepository;
        private readonly IMapper _mapper;
        protected ApiResponse _apiResponse;

        public TownController(ILogger<TownController> logger, ITownRepository townRepository, IMapper mapper)
        {
            _logger = logger;
            _townRepository = townRepository;
            _mapper = mapper;
            _apiResponse = new();
        }

        [HttpGet]
        [ResponseCache(CacheProfileName = "Default30")]
        [Authorize(Roles = "admin", AuthenticationSchemes = "Bearer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse>> GetTowns()
        {
            try
            {
                IEnumerable<Town> towns = await _townRepository.GetAll();

                _apiResponse.Result = _mapper.Map<IEnumerable<TownDto>>(towns);
                _apiResponse.StatusCode = HttpStatusCode.OK;

                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.Success = false;
                _apiResponse.ErrorMessages = new List<string> { ex.ToString() };
            }

            return _apiResponse;
        }

        [HttpGet("TownsPaged")]
        [ResponseCache(CacheProfileName = "Default30")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<ApiResponse> GetTownsPaged([FromQuery] Parameters parameters)
        {
            try
            {
                var towns = _townRepository.GetAllPaged(parameters);

                _apiResponse.Result = _mapper.Map<IEnumerable<TownDto>>(towns);
                _apiResponse.StatusCode = HttpStatusCode.OK;
                _apiResponse.TotalPages = towns.MetaData!.TotalPages;

                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.Success = false;
                _apiResponse.ErrorMessages = new List<string> { ex.ToString() };
            }

            return _apiResponse;
        }

        [HttpGet("{id:int}", Name = "GetTown")]
        [Authorize(Roles = "admin", AuthenticationSchemes = "Bearer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse>> GetTown(int id)
        {
            try
            {
                if (id == 0)
                {
                    _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                    _apiResponse.Success = false;

                    return BadRequest(_apiResponse);
                }

                var town = await _townRepository.Get(t => t.Id == id);

                if (town == null)
                {
                    _apiResponse.StatusCode = HttpStatusCode.NotFound;
                    _apiResponse.Success = false;

                    return NotFound(_apiResponse);
                }

                _apiResponse.Result = _mapper.Map<TownDto>(town);
                _apiResponse.StatusCode = HttpStatusCode.OK;

                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.Success = false;
                _apiResponse.ErrorMessages = new List<string> { ex.ToString() };
            }

            return _apiResponse;
        }

        [HttpPost]
        [Authorize(Roles = "admin", AuthenticationSchemes = "Bearer")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse>> CreateTown([FromBody] TownRequestDto townRequestDto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                if (await _townRepository.Get(t => t.Name!.ToLower() == townRequestDto.Name!.ToLower()) != null)
                {
                    ModelState.AddModelError("ErrorMessages", "La villa con ese nombre ya existe.");

                    return BadRequest(ModelState);
                }

                if (townRequestDto == null) return BadRequest(ModelState);

                Town town = _mapper.Map<Town>(townRequestDto);

                town.CreationDate = DateTime.Now;
                town.UpdateDate = DateTime.Now;

                await _townRepository.Create(town);

                _apiResponse.Result = town;
                _apiResponse.StatusCode = HttpStatusCode.Created;

                return CreatedAtRoute("GetTown", new { id = town.Id }, _apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.Success = false;
                _apiResponse.ErrorMessages = new List<string> { ex.ToString() };
            }

            return _apiResponse;
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "admin", AuthenticationSchemes = "Bearer")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse>> UpdateTown(int id, [FromBody] TownUpdateDto townUpdateDto)
        {
            if (townUpdateDto == null || id != townUpdateDto.Id)
            {
                _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                _apiResponse.Success = false;

                return BadRequest(_apiResponse);
            }

            Town town = _mapper.Map<Town>(townUpdateDto);

            await _townRepository.Update(town);

            _apiResponse.StatusCode = HttpStatusCode.NoContent;

            return _apiResponse;
        }

        [HttpPatch("{id:int}")]
        [Authorize(Roles = "admin", AuthenticationSchemes = "Bearer")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse>> UpdatePartialTown(int id, JsonPatchDocument<TownUpdateDto> patchDto)
        {
            if (patchDto == null || id == 0) return BadRequest();

            var town = await _townRepository.Get(t => t.Id == id, tracked: false);

            TownUpdateDto townUpdateDto = _mapper.Map<TownUpdateDto>(town);

            if (town == null) return BadRequest();

            patchDto.ApplyTo(townUpdateDto, ModelState);

            if (!ModelState.IsValid) return BadRequest(ModelState);

            Town townToPatch = _mapper.Map<Town>(townUpdateDto);

            await _townRepository.Update(townToPatch);

            _apiResponse.StatusCode = HttpStatusCode.NoContent;

            return Ok(_apiResponse);
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "admin", AuthenticationSchemes = "Bearer")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteTown(int id)
        {
            try
            {
                if (id == 0)
                {
                    _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                    _apiResponse.Success = false;

                    return BadRequest(_apiResponse);
                }

                var town = await _townRepository.Get(t => t.Id == id);

                if (town == null)
                {
                    _apiResponse.StatusCode = HttpStatusCode.NotFound;
                    _apiResponse.Success = false;

                    return NotFound(_apiResponse);
                }

                await _townRepository.Remove(town);

                _apiResponse.StatusCode = HttpStatusCode.NoContent;

                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.Success = false;
                _apiResponse.ErrorMessages = new List<string> { ex.ToString() };
            }

            return BadRequest(_apiResponse);
        }
    }
}