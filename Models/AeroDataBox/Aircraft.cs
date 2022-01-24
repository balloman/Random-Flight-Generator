// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Random_Realistic_Flight.Models.AeroDataBox;

// ReSharper disable once ClassNeverInstantiated.Global
public record Aircraft
{
    public string? ModeS { get; set; }
    public string? Registration { get; set; }
    public string Model { get; set; } = "";
}
