namespace Random_Realistic_Flight.Models.Interfaces;

/// <summary>
///     An individual aircraft and it's relevant properties
/// </summary>
public interface IAircraft
{
    /// <summary>The aircraft's Model Name</summary>
    public string Model { get; }

    /// <summary>The registration of the particular aircraft if it can be found</summary>
    public string? Registration { get; }
}
