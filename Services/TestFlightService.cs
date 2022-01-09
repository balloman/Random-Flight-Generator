// using System.Text.Json.Nodes;
// using Random_Realistic_Flight.Models;
// using Random_Realistic_Flight.Services.Interfaces;
//
// namespace Random_Realistic_Flight.Services;
//
// public class TestFlightService : IFlightService
// {
//     private readonly ILogger<TestFlightService> _logger;
//
//     public TestFlightService(ILogger<TestFlightService> logger)
//     {
//         _logger = logger;
//     }
//     
//     /// <inheritdoc />
//     public async Task<IEnumerable<Departure>?> GetDeparturesAsync(string airportCode, TimeSpan timeBack)
//     {
//         throw new NotImplementedException();
//     }
//
//     /// <inheritdoc />
//     public async Task<IEnumerable<Aircraft>> GetAircraftByAirportAsync(string airportCode, TimeSpan timeBack)
//     {
//         _logger.LogInformation(
//             "Getting aircraft for airport {AirportCode} and timespan {TimeBack}", airportCode, timeBack);
//         return new[]
//         {
//             new Aircraft("someReg", "sdfs", "Airbus A320"),
//             new Aircraft("someOtherReg", "sfds", "Boeing 737-800")
//         };
//     }
// }
