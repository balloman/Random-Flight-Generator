using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Random_Realistic_Flight.Models.Dtos;

// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable CollectionNeverUpdated.Local
// ReSharper disable UnusedAutoPropertyAccessor.Local
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable CollectionNeverUpdated.Global

namespace Random_Realistic_Flight.Models.AeroDataBox;

public record Departure
{
    public Aircraft Aircraft { get; set; } = default!;
    [JsonPropertyName("departure")] public DepartureStats DepartureStats { get; set; } = default!;
    public JsonObject Arrival { get; set; } = default!;

    [JsonIgnore]
    public Airport ArrivalAirport => Arrival["airport"].Deserialize<Airport>(
        new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;

    [JsonPropertyName("number")] public string FlightNumber { get; set; } = "";
    internal AirportDto? DepartureAirport { get; set; }

    public record Airport
    {
        public string Iata { get; set; } = "";
        public string Icao { get; set; } = "";
        public string Name { get; set; } = "";
    }
}
