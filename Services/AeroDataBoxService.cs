using System.Collections.Immutable;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Nodes;
using Random_Realistic_Flight.Models;
using Random_Realistic_Flight.Services.Interfaces;

namespace Random_Realistic_Flight.Services;

public class AeroDataBoxService : IFlightService
{
    private const string BASE_ADDRESS = "https://aerodatabox.p.rapidapi.com/";
    
    private readonly HttpClient _client;
    private readonly ILogger<AeroDataBoxService> _logger;
    private readonly IKeyService _keyService;

    private Call? _lastAircraftCall;

    public AeroDataBoxService(ILogger<AeroDataBoxService> logger, IKeyService keyService)
    {
        _client = new HttpClient();
        _logger = logger;
        _keyService = keyService;
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

    /// <inheritdoc />
    public async Task<IEnumerable<Departure>?> GetDeparturesAsync(string airportCode, TimeSpan timeBack)
    {
        _logger.LogInformation("Getting departures from {AirportCode} at {TimeBack} time back", airportCode, timeBack);
        var localTime = await GetLocalTimeAsync(airportCode);
        var startTimeString = (localTime - timeBack).ToString("yyyy-MM-ddTHH:mm");
        var endTimeString = localTime.ToString("yyyy-MM-ddTHH:mm");
        var requestUri =
            $"flights/airports/icao/{airportCode}/{startTimeString}/{endTimeString}?withLeg=true&direction=Departure&withCancelled=false&withCargo=true&withPrivate=false&withLocation=false";
        var response = await _client.SendAsync(CraftRequest(requestUri));
        response.EnsureSuccessStatusCode();
        var jsonArray = (await response.Content.ReadFromJsonAsync<JsonObject>())?["departures"]?.AsArray();
        var departures = jsonArray.Deserialize<Departure[]>(new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
        return departures;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<string>> GetAircraftByAirportAsync(string airportCode, TimeSpan timeBack)
    {
        // Go ahead and cache the last call to this method.
        if (_lastAircraftCall != null && _lastAircraftCall.AirportCode == airportCode &&
            _lastAircraftCall.TimeBack == timeBack)
        {
            return _lastAircraftCall.Aircraft;
        }
        var departures = await GetDeparturesAsync(airportCode, timeBack);
        if (departures == null)
        {
            return ImmutableArray<string>.Empty;
        }
        var aircraft = new Dictionary<string, int>();
        foreach (var departure in departures)
        {
            var aircraftModel = departure.Aircraft?.Model;
            if (aircraftModel == null)
            {
                continue;
            }

            if (aircraft.ContainsKey(aircraftModel))
            {
                aircraft[aircraftModel]++;
            }else
            {
                aircraft.Add(aircraftModel, 1);
            }
        }

        var aircraftList = aircraft.OrderBy(pair => pair.Value).Reverse()
            .Select(pair => $"{pair.Key}: {pair.Value} Flight(s)");
        var aircraftByAirportAsync = aircraftList as string[] ?? aircraftList.ToArray();
        _lastAircraftCall = new Call(airportCode, timeBack, aircraftByAirportAsync);
        return aircraftByAirportAsync;
    }
    
    private record Call(string AirportCode, TimeSpan TimeBack, IEnumerable<string> Aircraft);
}
