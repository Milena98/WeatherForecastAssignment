using AutoMapper;
using Microsoft.Extensions.Options;
using WeatherForecast.App.Configuration;
using WeatherForecast.Core.Abstractions;
using WeatherForecast.Core.Boundaries.Requests;
using WeatherForecast.Core.Boundaries.Responses;

namespace WeatherForecast.App.Services
{
    public class GetHistoricalWeatherStrategy : BaseGetWeatherStrategy, IGetWeatherStrategy
    {
        public GetHistoricalWeatherStrategy(IHttpClientFactory factory, IOptions<WeatherSettings> options, IMapper mapper) 
            : base(factory, options, mapper) { }

        public bool CanProcess(DateTime date)
            => date.Date.CompareTo(DateTime.UtcNow.Date) < 0;

        public async Task<WeatherResponse> GetWeatherDataAsync(GetWeatherRequest request, CancellationToken cancellationToken)
        {
            var historicalData = await GetFromJson($"history.json?q={request.City}&dt={request.Date}", cancellationToken: cancellationToken);

            return historicalData;
        }
    }
}