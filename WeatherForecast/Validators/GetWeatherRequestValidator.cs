using FluentValidation;
using WeatherForecast.Core.Boundaries.Requests;

namespace WeatherForecast.Api.Validators
{
    public class GetWeatherRequestValidator : AbstractValidator<GetWeatherRequest>
    {
        public GetWeatherRequestValidator()
        {
            RuleFor(x => x.City)
                .NotEmpty()
                .NotNull()
                .Matches("^[A-Za-z\\s]+$");

            RuleFor(x => x.Date)
                .SetValidator(new WeatherDateValidator());         
        }
    }

    public class WeatherDateValidator : AbstractValidator<DateTime>
    {
        public WeatherDateValidator()
        {
            var maxDate = DateTime.Today.AddDays(14);
            var minDate = new DateTime(2015,1,1);

            RuleFor(x => x.Date)
                .LessThanOrEqualTo(maxDate)
                .GreaterThanOrEqualTo(minDate)
                .WithMessage("Forecast can be obtained for maximum 14 days ahead of the current date.");
        }
    }
}