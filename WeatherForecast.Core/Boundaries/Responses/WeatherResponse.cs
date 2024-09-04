namespace WeatherForecast.Core.Boundaries.Responses
{
    public class WeatherResponse
    {
        public List<WeatherDailyResponse> WeatherDaily { get; set; } = default!;
    }

    public class WeatherDailyResponse
    {
        public DateOnly Date { get; set; }
        public DailyResponse Day { get; set; } = default!;
    }

    public class DailyResponse
    {
        public string Condition { get; set; } = default!;
        public double MinTemperatureInCelzius { get; set; }
        public double MaxTemperatureInCelzius { get; set; }
        public double AvreageTemperatureInCelzius { get; set; }
        public double TotalPrecipitation { get; set; }
        public double TotalSnowInMm { get; set; }
        public double AverageHumidity { get; set; }
        public double DailyChanceOfRain { get; set; }
        public double UvIndex { get; set; }
    }
}