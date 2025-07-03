namespace MedrunnerApiClient.Models;

/// <summary>
/// Represents client stats for a person.
/// </summary>
public class ClientStats
{
    /// <summary>
    /// Emergency mission statistics for the client.
    /// </summary>
    public EmergencyStats Missions { get; set; }
}
