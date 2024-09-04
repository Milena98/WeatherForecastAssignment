using AutoMapper;
using Microsoft.Extensions.Options;
using Moq;
using System.Net.Http.Json;
using System.Net;
using WeatherForecast.App.Configuration;
using WeatherForecast.App.Exceptions;
using WeatherForecast.App.External;
using WeatherForecast.App.Services;
using WeatherForecast.App.UnitTests.Mocks;
using WeatherForecast.Core.Abstractions;
using WeatherForecast.Core.Boundaries.Requests;
using WeatherForecast.Core.Boundaries.Responses;
using WeatherForecast.App.UnitTests.Extensions;
using FluentAssertions;

namespace WeatherForecast.App.UnitTests.Services
{
    public class GetWeatherForecastStrategyTests
    {
        private IGetWeatherStrategy _sut;
        private Mock<IHttpClientFactory> _httpClientFactory;
        private Mock<IOptions<WeatherSettings>> _weatherSettings;
        private Mock<IMapper> _mapper;
        private GetWeatherRequest _request;
        private CancellationToken _cancellationToken;
        private Mock<HttpMessageHandler> _httpMessageHandlerMock;
        private ExternalWeatherDto _externalWeatherDto;
        private WeatherResponse _weatherResponse;


        [SetUp]
        public void Setup()
        {
            _httpClientFactory = new Mock<IHttpClientFactory>();
            _weatherSettings = new Mock<IOptions<WeatherSettings>>();
            _mapper = new Mock<IMapper>();
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            _externalWeatherDto = ModelMocks.CreateExternalWeatherDto;

            _httpMessageHandlerMock.SetupRequest()
                .ReturnsJsonResponse(_externalWeatherDto);

            var httpClient = new HttpClient(_httpMessageHandlerMock.Object)
            {
                BaseAddress = new Uri("https://someapi.com")
            };

            _httpClientFactory
                .Setup(x => x.CreateClient(It.IsAny<string>()))
                .Returns(httpClient);

            _weatherSettings
                .Setup(x => x.Value)
                .Returns(ModelMocks.CreateWeatherSettings);

            _weatherResponse = ModelMocks.CreateWeatherResponse;

            _mapper
                .Setup(x => x.Map<WeatherResponse>(It.IsAny<ForecastDto>()))
                .Returns(_weatherResponse);

            _sut = new GetWeatherForecastStrategy(_httpClientFactory.Object, _weatherSettings.Object, _mapper.Object);

            _request = ModelMocks.CreateGetWeatherRequest;
            _cancellationToken = new CancellationToken();
        }

        [Test]
        public void CanProcess_ShouldReturnTrue_WhenDateIsTodayOrInTheFuture()
        {
            var todaysDate = DateTime.UtcNow.Date;
            var futureDate = DateTime.UtcNow.AddDays(1).Date;

            Assert.That(_sut.CanProcess(futureDate), Is.True);
            Assert.That(_sut.CanProcess(todaysDate), Is.True);
        }

        [Test]
        public void CanProcess_ShouldReturnFalse_WhenDateIsInThePast()
        {
            var pastDate = DateTime.UtcNow.AddDays(-1);

            Assert.That(_sut.CanProcess(pastDate), Is.False);
        }

        [Test]
        public async Task GetWeatherDataAsync_ShouldCallDependenciesServicesOnce()
        {
            await _sut.GetWeatherDataAsync(_request, _cancellationToken);

            var forecast = _externalWeatherDto.Forecast;

            Func<object, bool> Match = _ =>
            {
                _.Should().BeEquivalentTo(forecast);
                return true;
            };

            _httpClientFactory.Verify(x => x.CreateClient(It.IsAny<string>()), Times.Once());
            _mapper.Verify(x => x.Map<WeatherResponse>(It.Is<ForecastDto>(x => Match(x))), Times.Once());
        }

        [Test]
        public async Task GetWeatherDataAsync_ShouldReturnRightWeather()
        {
            var weather = await _sut.GetWeatherDataAsync(_request, _cancellationToken);

            Assert.That(weather, Is.Not.Null);
            Assert.That(weather, Is.AssignableTo<WeatherResponse>());
            Assert.That(weather, Is.EqualTo(_weatherResponse));
        }

        [Test]
        public void GetWeatherDataAsync_ShouldThrowEntityNotFoundException_WhenDataIsNull()
        {
            var request = new GetWeatherRequest
            {
                City = "TestCity",
                Date = DateTime.UtcNow.AddDays(1)
            };
            var cancellationToken = CancellationToken.None;

            var numberOfDays = request.Date.Day - DateTime.UtcNow.Day + 1;

            ExternalWeatherDto? externalWeather = null;

            _httpMessageHandlerMock.SetupRequest()
                .ReturnsJsonResponse(externalWeather);

            Assert.ThrowsAsync<WeatherNotFoundException>(async () => await _sut.GetWeatherDataAsync(request, cancellationToken));
        }
    }
}
