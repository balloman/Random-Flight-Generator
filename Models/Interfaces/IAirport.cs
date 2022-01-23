namespace Random_Realistic_Flight.Models.Interfaces;

/// <summary>
///     Represents an individual airport and the relevant properties
/// </summary>
public interface IAirport
{
    /// <summary>
    ///     The name of the airport
    /// </summary>
    public string Name { get; }

    /// <summary>
    ///     The airport's ICAO code
    /// </summary>
    public string Icao { get; }
}
