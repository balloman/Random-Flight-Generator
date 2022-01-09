using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Random_Realistic_Flight.Models;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public record DepartureStats
{
    public string? ScheduledTimeUtc { get; set; }
    public string? ActualTimeUtc { get; set; }
}
