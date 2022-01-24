namespace Random_Realistic_Flight.Models.Dtos;

public record AirportDto(string Icao)
{
    public string? Name { get; set; }
}
