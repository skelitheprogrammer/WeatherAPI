using System.Globalization;
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

    public async Task<WeatherForecast> GetWeatherAsync(string location, DateOnly date)
    {
        HttpClient httpClient = _factory.CreateClient();
        Uri uri = CreateURI($"{_baseUrl}/{location}/{ParseString(date)}?unitGroup=metric&key={_key}");

        string stringAsync = await httpClient.GetStringAsync(uri);

        using JsonDocument document = JsonDocument.Parse(stringAsync);
        JsonElement root = document.RootElement;
        JsonElement daysArray = root.GetProperty("days")[0];

        DateOnly dateOnly = DateOnly.Parse(daysArray.GetProperty("datetime").GetString().AsSpan());
        double temp = daysArray.GetProperty("temp").GetDouble();

        return new(dateOnly, temp);
    }
    public async IAsyncEnumerable<WeatherForecast> GetWeatherAsync(string location, DateOnly fromDate, DateOnly toDate)
    {
        HttpClient client = _factory.CreateClient();
        Uri uri = CreateURI($"{_baseUrl}/{location}/{ParseString(fromDate)}/{ParseString(toDate)}?unitGroup=metric&key={_key}");

        string stringAsync = await client.GetStringAsync(uri);

        using JsonDocument document = JsonDocument.Parse(stringAsync);
        JsonElement root = document.RootElement;
        JsonElement daysArray = root.GetProperty("days");


        foreach (JsonElement jsonElement in daysArray.EnumerateArray())
        {
            DateOnly dateOnly = DateOnly.Parse(jsonElement.GetProperty("datetime").GetString().AsSpan());
            double temp = jsonElement.GetProperty("temp").GetDouble();

            yield return new(dateOnly, temp);
        }
    }

    private static Uri CreateURI(string address)
    {
        bool tryCreate = Uri.TryCreate(address, UriKind.Absolute, out Uri? uri);

        if (!tryCreate || uri is null)
        {
            throw new UriFormatException();
        }

        return uri;
    }

    private static string ParseString(DateOnly date) => date.ToString("yyyy-MM-dd");
}