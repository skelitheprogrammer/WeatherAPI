using WebAPI;
using WebAPI.WeatherService;
WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

IServiceCollection services = builder.Services;
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();
services.AddHttpClient();

services.AddSingleton<IWeatherService, WeatherService>();


WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapWeatherAPI();

app.Run();