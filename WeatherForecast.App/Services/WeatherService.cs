using WeatherForecast.Core.Abstractions;
using WeatherForecast.Core.Boundaries.Requests;
using WeatherForecast.Core.Boundaries.Responses;

namespace WeatherForecast.App.Services
{
    public class WeatherService : IWeatherService 
    {
        private readonly IEnumerable<IGetWeatherStrategy> _getWeatherStrategies;

        public WeatherService(IEnumerable<IGetWeatherStrategy> getWeatherStrategies)
        {
            _getWeatherStrategies = getWeatherStrategies;
        }

        public async Task<WeatherResponse> GetWeatherDataAsync(GetWeatherRequest request, CancellationToken cancellationToken)
        {
            var strategy = _getWeatherStrategies.First(x=> x.CanProcess(request.Date));

            return await strategy.GetWeatherDataAsync(request, cancellationToken);
        }
    }
}