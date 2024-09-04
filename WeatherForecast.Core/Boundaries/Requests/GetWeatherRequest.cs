namespace WeatherForecast.Core.Boundaries.Requests
{
    public class GetWeatherRequest
    {
        public string City { get; set; } = default!;
        public DateTime Date { get; set; }
    }
}