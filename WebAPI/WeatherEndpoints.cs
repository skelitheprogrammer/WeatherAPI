using Microsoft.AspNetCore.Http.HttpResults;
using WebAPI.WeatherService;
namespace WebAPI;

internal static class WeatherEndpoints
{
    internal static void MapWeatherAPI(this IEndpointRouteBuilder app)
    {
        app.MapGet("/{location}", GetWeather);

        static async Task<Results<Ok<WeatherForecast>, NotFound>> GetWeather(string location, IWeatherService service)
        {
            WeatherForecast weatherForecast = await service.GetWeatherAsync(location);
            return TypedResults.Ok(weatherForecast);
        }
    }
}