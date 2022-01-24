namespace Random_Realistic_Flight.Models.Dtos;

public record DepartureDto(AircraftDto Aircraft, string FlightNumber, AirportDto Origin, AirportDto Destination,
                           DateTime DepartureTimeUtc);
