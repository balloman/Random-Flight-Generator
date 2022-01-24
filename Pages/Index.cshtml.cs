using System.Collections.Immutable;
using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Random_Realistic_Flight.Extensions;
using Random_Realistic_Flight.Services.Interfaces;

// ReSharper disable UnassignedGetOnlyAutoProperty
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedMember.Global

namespace Random_Realistic_Flight.Pages;

public class IndexModel : PageModel
{
    private const string AIRCRAFTS_KEY = "ac";
    private readonly IFlightService _flightService;
    private readonly IKeyService _keyService;

    private readonly ILogger<IndexModel> _logger;
    private readonly Random _random = new();
    private readonly string[] _timeSpans = { "1 Hour", "6 Hours", "12 Hours" };

    public IndexModel(ILogger<IndexModel> logger, IFlightService flightService, IKeyService keyService)
    {
        _logger = logger;
        _flightService = flightService;
        _keyService = keyService;
        TimeSpansList = new SelectList(_timeSpans);
        SelectedTimeSpanItem = _timeSpans[0];
        Airport = "";
    }

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
    [BindProperty] public string SelectedAircraftItem { get; set; } = "";
    [BindProperty] public string Key { get; set; } = "";

    public async Task<IActionResult> OnPostGetAircraft()
    {
        if (!string.IsNullOrWhiteSpace(Key))
        {
            _keyService.Key = Key;
        } else
        {
            ViewData["Message"] = "Please enter a key";
            return Page();
        }

        var aircraft = await _flightService.GetAircraftByAirportAsync(Airport, SelectedTimeSpan);
        TempData.Set(AIRCRAFTS_KEY, aircraft);
        PopulateSelectList();
        return Page();
    }

    public void OnPostGetRandomFlight()
    {
        PopulateSelectList();
        var aircraft = TempData.Get<IImmutableSet<IFlightService.AircraftStats>>(AIRCRAFTS_KEY);
        if (aircraft == null)
        {
            return;
        }

        _logger.LogDebug("Getting a random flight for {Aircraft}", SelectedAircraftItem);
        var aircraftFlights = aircraft
            .First(stats => SelectedAircraftItem.Contains(stats.ModelName)).Departures;
        var randomIndex = _random.Next(0, aircraftFlights.Count);
        var randomFlight = aircraftFlights[randomIndex];
        ViewData["Message"] = $"Flight {randomFlight.FlightNumber} to {randomFlight.Destination.Icao}, " +
            $"Departing at {randomFlight.DepartureTimeUtc.ToUniversalTime().ToString(CultureInfo.CurrentCulture)} UTC";
    }

    private void PopulateSelectList()
    {
        _logger.LogDebug("Populating select list");
        var aircraft = TempData.Get<ImmutableHashSet<IFlightService.AircraftStats>>(AIRCRAFTS_KEY);
        if (aircraft == null)
        {
            return;
        }

        var aircraftStringList = aircraft
            .OrderByDescending(stats => stats.Departures.Count)
            .ThenBy(stats => stats.ModelName)
            .Select(stats => $"{stats.ModelName} - {stats.Departures.Count} Flights")
            .ToImmutableList();
        AircraftList = new SelectList(aircraftStringList);
    }
}
