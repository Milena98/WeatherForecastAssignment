using AutoBogus;
using WeatherForecast.App.Configuration;
using WeatherForecast.App.External;
using WeatherForecast.Core.Boundaries.Requests;
using WeatherForecast.Core.Boundaries.Responses;

namespace WeatherForecast.App.UnitTests.Mocks
{
    public static class ModelMocks
    {
       
        public static ExternalWeatherDto CreateExternalWeatherDto
        =>    new AutoFaker<ExternalWeatherDto>()
                .RuleFor(x => x.Forecast, f => new AutoFaker<ForecastDto>()
                    .RuleFor(x => x.ForecastDay, f => new AutoFaker<ForecastDayDto>()
                        .RuleFor(fd => fd.Date, _ => DateOnly.FromDateTime(DateTime.UtcNow.Date))
                        .Generate(1))
                    .Generate())
                .Generate();

        public static WeatherResponse CreateWeatherResponse
            => new AutoFaker<WeatherResponse>()
                .RuleFor(x => x.WeatherDaily, f => new AutoFaker<WeatherDailyResponse>()
                .RuleFor(wd => wd.Date, _ => DateOnly.FromDateTime(DateTime.UtcNow.Date))
                .RuleFor(wd => wd.Day, f => new AutoFaker<DailyResponse>()).Generate(1));

        public static WeatherSettings CreateWeatherSettings
            => new AutoFaker<WeatherSettings>().RuleFor(x=>x.BaseUrl, "https://someurl.com");

        public static GetWeatherRequest CreateGetWeatherRequest
            => new AutoFaker<GetWeatherRequest>();
    }
}