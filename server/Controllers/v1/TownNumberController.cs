using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using server.Dtos;
using server.Models;
using server.Repositories;

namespace server.Controllers.v1
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class TownNumberController : ControllerBase
    {
        private readonly ITownRepository _townRepository;
        private readonly ITownNumberRepository _townNumberRepository;
        private readonly IMapper _mapper;
        protected ApiResponse _apiResponse;

        public TownNumberController(ITownRepository townRepository, ITownNumberRepository townNumberRepository, IMapper mapper)
        {
            _townRepository = townRepository;
            _townNumberRepository = townNumberRepository;
            _mapper = mapper;
            _apiResponse = new();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Roles = "Admin", AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<ApiResponse>> GetTownsNumber()
        {
            try
            {
                IEnumerable<TownNumber> townsNumber = await _townNumberRepository.GetAll(includeProperties: "Town");

                _apiResponse.Result = _mapper.Map<IEnumerable<TownNumberResponseDto>>(townsNumber);
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

        [HttpGet("{id:int}", Name = "GetTownNumber")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Admin", AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<ApiResponse>> GetTownNumber(int id)
        {
            try
            {
                if (id == 0)
                {
                    _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                    _apiResponse.Success = false;

                    return BadRequest(_apiResponse);
                }

                var townNumber = await _townNumberRepository.Get(t => t.TownNo == id, includeProperties: "Town");

                if (townNumber == null)
                {
                    _apiResponse.StatusCode = HttpStatusCode.NotFound;
                    _apiResponse.Success = false;

                    return NotFound(_apiResponse);
                }

                _apiResponse.Result = _mapper.Map<TownNumberResponseDto>(townNumber);
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
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = "Admin", AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<ApiResponse>> CreateTownNumber([FromBody] TownNumberRequestDto townNumberRequestDto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                if (await _townNumberRepository.Get(tn => tn.TownNo == townNumberRequestDto.TownNo) != null)
                {
                    ModelState.AddModelError("ErrorMessages", "El número de la villa ya existe");

                    return BadRequest(ModelState);
                }

                if (await _townRepository.Get(t => t.Id == townNumberRequestDto.TownId) == null)
                {
                    ModelState.AddModelError("ErrorMessages", "El Id de la villa no existe.");

                    return BadRequest(ModelState);
                }

                if (townNumberRequestDto == null) return BadRequest(ModelState);

                TownNumber townNumber = _mapper.Map<TownNumber>(townNumberRequestDto);

                townNumber.CreationDate = DateTime.Now;
                townNumber.UpdateDate = DateTime.Now;

                await _townNumberRepository.Create(townNumber);

                _apiResponse.Result = townNumber;
                _apiResponse.StatusCode = HttpStatusCode.Created;

                return CreatedAtRoute("GetTownNumber", new { id = townNumber.TownNo }, _apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.Success = false;
                _apiResponse.ErrorMessages = new List<string> { ex.ToString() };
            }

            return _apiResponse;
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "Admin", AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<ApiResponse>> UpdateTownNumber(int id, [FromBody] TownNumberUpdateDto townNumberUpdateDto)
        {
            if (townNumberUpdateDto == null || id != townNumberUpdateDto.TownNo)
            {
                _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                _apiResponse.Success = false;

                return BadRequest(_apiResponse);
            }

            if (await _townRepository.Get(t => t.Id == townNumberUpdateDto.TownId) == null)
            {
                ModelState.AddModelError("ErrorMessages", "El Id de la villa no existe.");

                return BadRequest(ModelState);
            }

            TownNumber townNumber = _mapper.Map<TownNumber>(townNumberUpdateDto);

            await _townNumberRepository.Update(townNumber);

            _apiResponse.StatusCode = HttpStatusCode.NoContent;

            return Ok(_apiResponse);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Admin", AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<ApiResponse>> DeleteTownNumber(int id)
        {
            try
            {
                if (id == 0)
                {
                    _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                    _apiResponse.Success = false;

                    return BadRequest(_apiResponse);
                }

                var townNumber = await _townNumberRepository.Get(t => t.TownNo == id);

                if (townNumber == null)
                {
                    _apiResponse.StatusCode = HttpStatusCode.NotFound;
                    _apiResponse.Success = false;

                    return NotFound(_apiResponse);
                }

                await _townNumberRepository.Remove(townNumber);

                _apiResponse.StatusCode = HttpStatusCode.NoContent;

                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.Success = false;
                _apiResponse.ErrorMessages = new List<string> { ex.ToString() };
            }

            return _apiResponse;
        }
    }
}