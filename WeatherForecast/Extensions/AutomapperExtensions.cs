using AutoMapper;
using WeatherForecast.App.Helpers.Profiles;

namespace WeatherForecast.Api.Extensions
{
    public static class AutomapperExtensions
    {
        public static void AddAutoMapper(this IServiceCollection services)
        {
            var appAssembly = typeof(WeatherResponseProfile).Assembly;

            var config = new MapperConfiguration(cfg => { cfg.AddMaps(appAssembly); });

            IMapper mapper = config.CreateMapper();
            services.AddSingleton(mapper);
        }
    }
}