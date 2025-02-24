namespace WebAPI.WeatherService;

public record WeatherForecast(DateOnly Date, double TemperatureC)
{
    public double TemperatureF => 32 + TemperatureC / 0.5556;
}