namespace WebAPI.WeatherService;

public interface IWeatherService
{
    Task<WeatherForecast> GetWeatherAsync(string location, DateOnly date);
    IAsyncEnumerable<WeatherForecast> GetWeatherAsync(string location, DateOnly fromDate, DateOnly toDate);
}