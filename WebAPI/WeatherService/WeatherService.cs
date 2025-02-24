using System.Text.Json;
namespace WebAPI.WeatherService;

internal sealed class WeatherService : IWeatherService
{
    private readonly IHttpClientFactory _factory;

    private readonly string _key;
    private readonly string _baseUrl;

    public WeatherService(IHttpClientFactory factory, IConfiguration configuration)
    {
        _factory = factory;
        _key = configuration["WeatherAPI:Key"];
        _baseUrl = configuration["WeatherAPI:BaseUrl"];
    }

    public async Task<WeatherForecast> GetWeatherAsync(string location)
    {
        HttpClient httpClient = _factory.CreateClient();

        string stringAsync = await httpClient.GetStringAsync($"{_baseUrl}/{location}?key={_key}");

        using JsonDocument document = JsonDocument.Parse(stringAsync);
        JsonElement root = document.RootElement;
        JsonElement daysArray = root.GetProperty("days")[0];

        DateOnly dateOnly = DateOnly.Parse(daysArray.GetProperty("datetime").GetString().AsSpan());
        double temp = daysArray.GetProperty("temp").GetDouble();

        return new(dateOnly, temp);
    }
}