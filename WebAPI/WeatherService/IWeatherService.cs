namespace WebAPI.WeatherService;

public interface IWeatherService
{
    Task<WeatherForecast> GetWeatherAsync(string location);
}