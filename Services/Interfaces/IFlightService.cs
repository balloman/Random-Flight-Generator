using System.Collections.Immutable;
using Random_Realistic_Flight.Models.Dtos;

namespace Random_Realistic_Flight.Services.Interfaces;

public interface IFlightService
{
    /// <summary>
    /// Gets all the aircraft models at a given airport in a time period
    /// </summary>
    /// <param name="airportCode">The ICAO code for the airport</param>
    /// <param name="timeBack">How far back to go</param>
    /// <returns>A set of the aircraft models with their respective flights for that range</returns>
    Task<IImmutableList<AircraftStats>> GetAircraftByAirportAsync(string airportCode, TimeSpan timeBack);

    public record AircraftStats(string ModelName, IImmutableList<DepartureDto> Departures);
}
