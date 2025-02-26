using Microsoft.AspNetCore.Http.HttpResults;
using WebAPI.WeatherService;
namespace WebAPI;

internal static class WeatherEndpoints
{
    internal static void MapWeatherAPI(this IEndpointRouteBuilder app)
    {
        app.MapGet("/{location}/{date:datetime}", GetWeather);
        app.MapGet("/{location}/{fromDate:datetime}/{toDate:datetime}", GetWeatherForecast);

        static async Task<Results<Ok<WeatherForecast>, NotFound>> GetWeather(string location, DateTime date, IWeatherService service)
        {
            WeatherForecast weatherForecast = await service.GetWeatherAsync(location, DateOnly.FromDateTime(date));
            return TypedResults.Ok(weatherForecast);
        }

        static async Task<IResult> GetWeatherForecast(string location, DateTime fromDate, DateTime toDate, IWeatherService service)
        {
            return TypedResults.Ok(service.GetWeatherAsync(location, DateOnly.FromDateTime(fromDate), DateOnly.FromDateTime(toDate)));
        }
    }
}