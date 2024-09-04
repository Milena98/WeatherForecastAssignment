
using System.Text.Json.Serialization;

namespace WeatherForecast.App.External
{
    public class ExternalWeatherDto
    {
        public ForecastDto Forecast { get; set; } = default!;
    }

    public class ForecastDto
    {
        public List<ForecastDayDto> ForecastDay { get; set; } = default!;
    }

    public class ForecastDayDto
    {
        public DateOnly Date { get; set; }  
        public DailyDto Day { get; set; } = default!;
    }

    public class DailyDto
    {
        [JsonPropertyName("Mintemp_c")]
        public double MinTemp { get; set; }

        [JsonPropertyName("Maxtemp_c")]
        public double MaxTemp { get; set; }

        [JsonPropertyName("Avgtemp_c")]
        public double AvgTemp { get; set; }

        [JsonPropertyName("TotalPrecip_mm")]
        public double TotalPrecipitation {  get; set; }

        [JsonPropertyName("TotalSnow_mm")]
        public double TotalSnow {  get; set; }

        public double Avghumidity {  get; set; }

        [JsonPropertyName("Daily_chance_of_rain")]
        public double DailyChanceOfRain {  get; set; }

        public double Uv { get; set; }

        public ConditionDto Condition { get; set; } = default!;
    }

    public class ConditionDto
    {
        public string Text { get; set; } = default!;
    }
}