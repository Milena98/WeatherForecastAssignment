using Microsoft.AspNetCore.Mvc;
using WeatherForecast.Core.Abstractions;
using WeatherForecast.Core.Boundaries.Requests;

namespace WeatherForecast.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherController : ControllerBase
    {
        private readonly IWeatherService _weatherService;

        public WeatherController(IWeatherService weatherService)
        {
            _weatherService = weatherService;
        }
        
        [HttpGet]
        [ProducesResponseType<GetWeatherRequest>(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetCurrentWeather([FromQuery] GetWeatherRequest request)
        {
            var forecast = await _weatherService.GetWeatherDataAsync(request, HttpContext.RequestAborted);
            return Ok(forecast);
        }
    }
}