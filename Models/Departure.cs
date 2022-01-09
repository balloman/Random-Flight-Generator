using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable CollectionNeverUpdated.Local
// ReSharper disable UnusedAutoPropertyAccessor.Local
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable CollectionNeverUpdated.Global

namespace Random_Realistic_Flight.Models;

public record Departure
{
    
    public Aircraft? Aircraft { get; set; }
    public string? Number { get; set; }
    [JsonPropertyName("departure")]
    public DepartureStats? DepartureStats { get; set; }
    public JsonObject? Arrival { get; set; }

    [JsonIgnore] public Airport? ArrivalAirport => Arrival?["airport"]?.Deserialize<Airport>(new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

    public record Airport
    {
        public string? Icao { get; set; }
        public string? Iata { get; set; }
        public string? Name { get; set; }
        
    }
}
