using WeatherForecast.Api.Extensions;
using WeatherForecast.App.Extensions;

var builder = WebApplication.CreateBuilder(args);

var configuration = GetConfiguration();

ResolveServices(builder, configuration);

static IConfiguration GetConfiguration()
=> new ConfigurationBuilder()
      .SetBasePath(Directory.GetCurrentDirectory())
      .AddJsonFile("appsettings.json")
      .Build();

static void ResolveServices(WebApplicationBuilder builder, IConfiguration configuration)
{
    builder.Services.AddServices();
    builder.Services.AddOptionsConfigurations(configuration);
    builder.Services.ConfigureFluentValidators();
    builder.Services.AddAutoMapper();
    builder.Services.AddHttpClient();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddControllers();
    builder.Services.AddSwaggerGen();
    var app = builder.Build();

    app.ConfigureExceptionMiddleware();
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}