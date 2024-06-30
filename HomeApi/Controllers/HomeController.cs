using AutoMapper;
using HomeApi.Configuration;
using HomeApi.Contracts.Models.Home;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace HomeApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : Controller
    {
        private readonly IOptions<HomeOptions> _options;
        private readonly IMapper _mapper;

        public HomeController(IOptions<HomeOptions> options, IMapper mapper) 
        {
            _options = options;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("info")]
        public IActionResult Info()
        {
            var infoResponse = _mapper.Map<HomeOptions, InfoResponse>(_options.Value);

            return StatusCode(200, infoResponse);
        }
    }
}
