using AutoMapper;
using WeatherForecast.App.External;
using WeatherForecast.Core.Boundaries.Responses;

namespace WeatherForecast.App.Helpers.Profiles
{
    public class WeatherResponseProfile : Profile
    {
        public WeatherResponseProfile()
        {
            CreateMap<ForecastDto, WeatherResponse>().ForMember(dest=>dest.WeatherDaily, opt => opt.MapFrom(src=>src.ForecastDay));
            CreateMap<ForecastDayDto, WeatherDailyResponse>();
            var dailyMap = CreateMap<DailyDto, DailyResponse>();
            MapProperties(dailyMap);
        }

        private static void MapProperties(IMappingExpression<DailyDto, DailyResponse> map)
        {
            map.ForMember(dest => dest.Condition, opt => opt.MapFrom(src => src.Condition.Text));
            map.ForMember(dest => dest.MinTemperatureInCelzius, opt => opt.MapFrom(src => src.MinTemp));
            map.ForMember(dest => dest.MaxTemperatureInCelzius, opt => opt.MapFrom(src => src.MaxTemp));
            map.ForMember(dest => dest.AvreageTemperatureInCelzius, opt => opt.MapFrom(src => src.AvgTemp));
            map.ForMember(dest => dest.TotalPrecipitation, opt => opt.MapFrom(src => src.TotalPrecipitation));
            map.ForMember(dest => dest.TotalSnowInMm, opt => opt.MapFrom(src => src.TotalSnow));
            map.ForMember(dest => dest.AverageHumidity, opt => opt.MapFrom(src => src.Avghumidity));
            map.ForMember(dest => dest.UvIndex, opt => opt.MapFrom(src => src.Uv));
        } 
    }
}