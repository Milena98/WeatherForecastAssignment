using AutoMapper;
using Microsoft.Extensions.Options;
using WeatherForecast.App.Configuration;
using WeatherForecast.Core.Abstractions;
using WeatherForecast.Core.Boundaries.Requests;
using WeatherForecast.Core.Boundaries.Responses;

namespace WeatherForecast.App.Services
{
    public class GetWeatherForecastStrategy : BaseGetWeatherStrategy, IGetWeatherStrategy
    {
        public GetWeatherForecastStrategy(IHttpClientFactory factory, IOptions<WeatherSettings> options, IMapper mapper)
            : base(factory, options, mapper) {  }

        public bool CanProcess(DateTime date)
            => date.Date.CompareTo(DateTime.UtcNow.Date) >= 0;

        public async Task<WeatherResponse> GetWeatherDataAsync(GetWeatherRequest request, CancellationToken cancellationToken)
        {
            var forecastDays = request.Date.Day - DateTime.UtcNow.Day + 1;

            var forecast = await GetFromJson($"forecast.json?q={request.City}&days={forecastDays}", cancellationToken: cancellationToken);

            return forecast;
        }
    }
}