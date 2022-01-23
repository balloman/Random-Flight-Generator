// ReSharper disable UnusedAutoPropertyAccessor.Global

using Random_Realistic_Flight.Models.Interfaces;

namespace Random_Realistic_Flight.Models.AeroDataBox;

// ReSharper disable once ClassNeverInstantiated.Global
public record Aircraft : IAircraft
{
    public string? ModeS { get; set; }
    public string? Registration { get; set; }
    public string Model { get; set; } = "";
}
