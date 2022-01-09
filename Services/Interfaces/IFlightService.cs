using Random_Realistic_Flight.Models;

namespace Random_Realistic_Flight.Services.Interfaces;

public interface IFlightService
{
    /// <summary>
    /// Gets all the departures for a given airport in a time period
    /// </summary>
    /// <param name="airportCode">The ICAO code for the airport</param>
    /// <param name="timeBack">How far back to go</param>
    /// <returns>A list of departure objects</returns>
    Task<IEnumerable<Departure>?> GetDeparturesAsync(string airportCode, TimeSpan timeBack);

    /// <summary>
    /// Gets all the aircraft at a given airport in a time period
    /// </summary>
    /// <param name="airportCode">The ICAO code for the airport</param>
    /// <param name="timeBack">How far back to go</param>
    /// <returns></returns>
    Task<IEnumerable<string>> GetAircraftByAirportAsync(string airportCode, TimeSpan timeBack);
}
