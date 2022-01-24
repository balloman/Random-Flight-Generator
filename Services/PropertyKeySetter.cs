using Random_Realistic_Flight.Services.Interfaces;

namespace Random_Realistic_Flight.Services;

public class PropertyKeySetter : IKeyService
{
    /// <inheritdoc />
    public string Key { get; set; } = "";
}
