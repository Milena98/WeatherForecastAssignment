using FluentValidation.AspNetCore;
using FluentValidation;
using WeatherForecast.Api.Validators;

namespace WeatherForecast.Api.Extensions
{
    public static class FluentValidationsExtensions
    {
        public static void ConfigureFluentValidators(this IServiceCollection services)
        {
            services.AddValidatorsFromAssemblyContaining<GetWeatherRequestValidator>();
            services.AddFluentValidationAutoValidation();
        }
    }
}