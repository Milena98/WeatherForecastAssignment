using WeatherForecast.Core.Boundaries.Requests;
using WeatherForecast.Core.Boundaries.Responses;

namespace WeatherForecast.Core.Abstractions
{
    public interface IGetWeatherStrategy
    {
        bool CanProcess(DateTime date);
        Task<WeatherResponse> GetWeatherDataAsync(GetWeatherRequest request, CancellationToken cancellationToken);
    }
}