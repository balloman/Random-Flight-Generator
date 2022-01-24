using System.Collections.Immutable;
using System.Text.Json;
using System.Text.Json.Nodes;
using Random_Realistic_Flight.Models.AeroDataBox;
using Random_Realistic_Flight.Models.Dtos;
using Random_Realistic_Flight.Services.Interfaces;

namespace Random_Realistic_Flight.Services;

public class AeroDataBoxService : IFlightService
{
    private const string BASE_ADDRESS = "https://aerodatabox.p.rapidapi.com/";

    private readonly HttpClient _client;
    private readonly IKeyService _keyService;
    private readonly ILogger<AeroDataBoxService> _logger;

    public AeroDataBoxService(ILogger<AeroDataBoxService> logger, IKeyService keyService)
    {
        _client = new HttpClient();
        _logger = logger;
        _keyService = keyService;
    }

    /// <inheritdoc />
    public async Task<IImmutableList<IFlightService.AircraftStats>> GetAircraftByAirportAsync(
        string airportCode, TimeSpan timeBack)
    {
        var departures = await GetDeparturesAsync(airportCode, timeBack);
        if (departures == null)
        {
            return ImmutableArray<IFlightService.AircraftStats>.Empty;
        }

        var modelNames = departures.Select(d => d.Aircraft.Model).Distinct();
        var statsList = modelNames.Select(name => new IFlightService.AircraftStats(
            name,
            departures.Where(d => d.Aircraft.Model == name).ToImmutableList()));

        return statsList.ToImmutableList();
    }

    private HttpRequestMessage CraftRequest(string requestUri)
    {
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri(BASE_ADDRESS + requestUri),
            Headers =
            {
                { "x-rapidapi-key", _keyService.Key },
                { "x-rapidapi-host", "aerodatabox.p.rapidapi.com" }
            }
        };
        return request;
    }

    private async Task<DateTimeOffset> GetLocalTimeAsync(string airportCode)
    {
        var requestUri = $"airports/icao/{airportCode}/time/local";
        var response = await _client.SendAsync(CraftRequest(requestUri));
        var content = await response.Content.ReadFromJsonAsync<JsonObject>();
        var dateTimeString = content?["localTime"]?.ToString();
        var dateTime = DateTimeOffset.Parse(dateTimeString!);
        return dateTime;
    }

    private async Task<IImmutableList<DepartureDto>?> GetDeparturesAsync(string airportCode, TimeSpan timeBack)
    {
        _logger.LogInformation("Getting departures from {AirportCode} at {TimeBack} time back", airportCode, timeBack);
        var localTime = await GetLocalTimeAsync(airportCode);
        var startTimeString = (localTime - timeBack).ToString("yyyy-MM-ddTHH:mm");
        var endTimeString = localTime.ToString("yyyy-MM-ddTHH:mm");
        var departureAirport = new AirportDto(airportCode);
        var requestUri =
            $"flights/airports/icao/{airportCode}/{startTimeString}/{endTimeString}?withLeg=true&direction=Departure&withCancelled=false&withCargo=true&withPrivate=false&withLocation=false";
        var response = await _client.SendAsync(CraftRequest(requestUri));
        response.EnsureSuccessStatusCode();
        var jsonArray = (await response.Content.ReadFromJsonAsync<JsonObject>())?["departures"]?.AsArray();
        var departures = jsonArray.Deserialize<Departure[]>(new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
        return departures?.Select(d =>
        {
            var aircraftDto = new AircraftDto(d.Aircraft.Model);
            var departureDto = new DepartureDto(aircraftDto,
                d.FlightNumber,
                departureAirport,
                new AirportDto(d.ArrivalAirport.Icao),
                DateTime.Parse(d.DepartureStats.ScheduledTimeUtc!));
            return departureDto;
        }).ToImmutableArray();
    }
}
