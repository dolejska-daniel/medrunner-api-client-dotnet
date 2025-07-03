namespace Arkanis.External.MedRunner.Models;

/// <summary>
///     Enum for cancellation reasons.
/// </summary>
public enum CancellationReason
{
    None,
    Other,
    SuccumbedToWounds,
    ServerError,
    Respawned,
    Rescued,
}
