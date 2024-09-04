using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WeatherForecast.App.Configuration;
using WeatherForecast.App.Services;
using WeatherForecast.Core.Abstractions;

namespace WeatherForecast.App.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IWeatherService, WeatherService>();
            services.AddScoped<IGetWeatherStrategy, GetHistoricalWeatherStrategy>();
            services.AddScoped<IGetWeatherStrategy, GetWeatherForecastStrategy>();
        }

        public static void AddOptionsConfigurations(this IServiceCollection services, IConfiguration configuration)
        {
            var forecastWebsiteConfig = configuration.GetSection("WeatherSettings");

            services.AddOptions();
            services.Configure<WeatherSettings>(forecastWebsiteConfig);
        }
    }
}