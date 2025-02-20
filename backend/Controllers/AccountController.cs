using Microsoft.AspNetCore.Mvc;

namespace WorkPlanner.Controllers
{
    [ApiController]
    [Route("api/accounts")]
    public class WeatherForecastController : ControllerBase
    {

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }


        [HttpPut("login")]
        public IActionResult Login()
        {
            return Ok();
        }

       

    }
}