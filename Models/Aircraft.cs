using System.Text.Json.Serialization;
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Random_Realistic_Flight.Models;

public record Aircraft
{
    public string? Registration { get; set; }
    public string? ModeS { get; set; }
    public string? Model { get; set; }

}
