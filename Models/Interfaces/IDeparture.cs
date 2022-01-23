namespace Random_Realistic_Flight.Models.Interfaces;

/// <summary>
///     A specific Departure for any flight
/// </summary>
public interface IDeparture
{
    /// <summary>The aircraft that is departing</summary>
    public IAircraft Aircraft { get; }

    /// <summary>The flight number of the departure</summary>
    public string FlightNumber { get; }

    /// <summary>The origin airport</summary>
    public IAirport Origin { get; }

    /// <summary>The destination airport</summary>
    public IAirport Destination { get; }

    /// <summary>The departure time</summary>
    public DateTime DepartureTimeUtc { get; }
}
