using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using server.Models;
using server.Repositories;

namespace server.Controllers.v2
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("2.0")]
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
        public IEnumerable<string> Get() {
            return new string[] { "valor1", "valor2" };
        }
    }
}