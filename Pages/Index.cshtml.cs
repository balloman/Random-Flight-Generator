using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Random_Realistic_Flight.Models;
using Random_Realistic_Flight.Services.Interfaces;
// ReSharper disable UnassignedGetOnlyAutoProperty
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedMember.Global

namespace Random_Realistic_Flight.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly IFlightService _flightService;
    private readonly IKeyService _keyService;
    private readonly string[] _timeSpans = { "1 Hour", "6 Hours", "12 Hours" };
    private readonly Random _random = new();

    [BindProperty] public string Airport { get; set; }
    public SelectList TimeSpansList { get; }
    [BindProperty] public string SelectedTimeSpanItem { get; set; }

    private TimeSpan SelectedTimeSpan
    {
        get
        {
            var timeSpanIdx = _timeSpans.ToList().IndexOf(SelectedTimeSpanItem);
            var selectedTimeSpan = timeSpanIdx switch
            {
                0 => TimeSpan.FromHours(1),
                1 => TimeSpan.FromHours(6),
                2 => TimeSpan.FromHours(12),
                _ => TimeSpan.FromHours(12)
            };
            return selectedTimeSpan;
        }
    }

    public SelectList AircraftList { get; set; } = new(Array.Empty<string>());
    [BindProperty] public string? SelectedAircraftItem { get; set; }
    public string OutputText { get; set; } = "";
    [BindProperty] public string Key { get; set; } = "";

    public IndexModel(ILogger<IndexModel> logger, IFlightService flightService, IKeyService keyService)
    {
        _logger = logger;
        _flightService = flightService;
        _keyService = keyService;
        TimeSpansList = new SelectList(_timeSpans);
        SelectedTimeSpanItem = _timeSpans[0];
        Airport = "";
    }

    public async Task<IActionResult> OnPostGetAircraft()
    {
        if (!string.IsNullOrWhiteSpace(Key))
        {
            _keyService.Key = Key;
        } else
        {
            OutputText = "Please enter a key.";
            return Page();
        }
        var aircraft = await _flightService.GetAircraftByAirportAsync(Airport, SelectedTimeSpan);
        AircraftList = new SelectList(aircraft);
        return Page();
    }

    public async Task<IActionResult> OnPostGetRandomFlight()
    {
        var flights = await _flightService.GetDeparturesAsync(Airport, SelectedTimeSpan);
        var flightsArray = flights as Departure[] ?? flights.ToArray();
        if (flightsArray.Length == 0)
        {
            OutputText = "No flights found";
            return Page();
        }

        flightsArray = flightsArray.Where(departure => !string.IsNullOrWhiteSpace(departure.Aircraft?.Model) && !string.IsNullOrWhiteSpace(SelectedAircraftItem) && SelectedAircraftItem.Contains(departure.Aircraft?.Model)).ToArray();
        if (flightsArray.Length == 0)
        {
            OutputText = "No flights found";
            return Page();
        }
        var randomIndex = _random.Next(0, flightsArray.Length);
        var randomFlight = flightsArray[randomIndex];
        OutputText = $"Flight {randomFlight.Number} to {randomFlight.ArrivalAirport?.Icao}\n" +
                     $"Departing at {randomFlight.DepartureStats.ScheduledTimeUtc}";
        return Page();
    }
}
