using AutoMapper;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;
using WeatherForecast.App.Configuration;
using WeatherForecast.App.Exceptions;
using WeatherForecast.App.External;
using WeatherForecast.Core.Boundaries.Responses;

namespace WeatherForecast.App.Services
{
    public abstract class BaseGetWeatherStrategy
    {
        private readonly IHttpClientFactory _factory;
        private readonly WeatherSettings _settings;
        private readonly IMapper _mapper;

        public BaseGetWeatherStrategy(IHttpClientFactory factory, IOptions<WeatherSettings> options, IMapper mapper)
        {
            _factory = factory;
            _settings = options.Value;
            _mapper = mapper;
        }

        protected async Task<WeatherResponse> GetFromJson(string query, CancellationToken cancellationToken)
        {
            var client = _factory.CreateClient();
            client.BaseAddress = new Uri(_settings.BaseUrl);

            var completeQuery = $"{query}&key={_settings.ApiKey}";

            var weather = await client.GetFromJsonAsync<ExternalWeatherDto>(completeQuery, cancellationToken: cancellationToken);

            return weather is null ?
                throw new WeatherNotFoundException($"Weather not found.") :
                _mapper.Map<WeatherResponse>(weather.Forecast);
        }
    }
}