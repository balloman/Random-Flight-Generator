namespace Random_Realistic_Flight.Models.Dtos;

public record AircraftDto(string Model)
{
    public string? Registration { get; init; }
}
