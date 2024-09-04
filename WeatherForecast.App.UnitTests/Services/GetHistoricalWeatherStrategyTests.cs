using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using WeatherForecast.App.Configuration;
using WeatherForecast.App.Exceptions;
using WeatherForecast.App.External;
using WeatherForecast.App.Services;
using WeatherForecast.App.UnitTests.Extensions;
using WeatherForecast.App.UnitTests.Mocks;
using WeatherForecast.Core.Abstractions;
using WeatherForecast.Core.Boundaries.Requests;
using WeatherForecast.Core.Boundaries.Responses;

namespace WeatherForecast.App.UnitTests.Services
{
    public class GetHistoricalWeatherStrategyTests
    {
        private IGetWeatherStrategy _sut;
        private Mock<IHttpClientFactory> _httpClientFactory;
        private Mock<IOptions<WeatherSettings>> _weatherSettingsMocks;
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
            _weatherSettingsMocks = new Mock<IOptions<WeatherSettings>>();
            _mapper = new Mock<IMapper>();
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>();

            _externalWeatherDto = ModelMocks.CreateExternalWeatherDto;
          
            _httpMessageHandlerMock.SetupRequest()
                .ReturnsJsonResponse(_externalWeatherDto);

            var weatherSettings = ModelMocks.CreateWeatherSettings;
            var httpClient = new HttpClient(_httpMessageHandlerMock.Object);
            httpClient.BaseAddress = new Uri(weatherSettings.BaseUrl);

            _httpClientFactory
                .Setup(x => x.CreateClient(It.IsAny<string>()))
                .Returns(httpClient);

            _weatherSettingsMocks
                .Setup(x => x.Value)
                .Returns(weatherSettings);

            _weatherResponse = ModelMocks.CreateWeatherResponse;

            _mapper
                .Setup(x => x.Map<WeatherResponse>(It.IsAny<ForecastDto>()))
                .Returns(_weatherResponse);

            _sut = new GetHistoricalWeatherStrategy(_httpClientFactory.Object, _weatherSettingsMocks.Object, _mapper.Object);

            _request = ModelMocks.CreateGetWeatherRequest;
            _cancellationToken = new CancellationToken();
        }

        [Test]
        public void CanProcess_ShouldReturnTrue_WhenDateIsInThePast()
        {
            var pastDate = DateTime.UtcNow.AddDays(-1);

            var result = _sut.CanProcess(pastDate);

            Assert.That(result, Is.True);
        }

        [Test]
        public void CanProcess_ShouldReturnFalse_WhenDateIsTodayOrInTheFuture()
        {
            var todaysDate = DateTime.UtcNow;
            var futureDate = DateTime.UtcNow.AddDays(1);

            Assert.That(_sut.CanProcess(todaysDate), Is.False);
            Assert.That(_sut.CanProcess(futureDate), Is.False);
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

            _httpClientFactory.Verify(x=>x.CreateClient(It.IsAny<string>()), Times.Once());
            _mapper.Verify(x=>x.Map<WeatherResponse>(It.Is<ForecastDto>(x=>Match(x))), Times.Once());  
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
                Date = DateTime.UtcNow.AddDays(-1)
            };
            var cancellationToken = CancellationToken.None;

            ExternalWeatherDto? externalWeather = null;

            _httpMessageHandlerMock.SetupRequest()
                .ReturnsJsonResponse(externalWeather);

            Assert.ThrowsAsync<WeatherNotFoundException>(async () => await _sut.GetWeatherDataAsync(request, cancellationToken));
        }
    }
}