using System.Diagnostics.CodeAnalysis;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Random_Realistic_Flight.Models.AeroDataBox;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public record DepartureStats
{
    public string? ScheduledTimeUtc { get; set; }
    public string? ActualTimeUtc { get; set; }
}
