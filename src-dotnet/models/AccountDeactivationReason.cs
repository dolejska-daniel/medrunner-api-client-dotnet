namespace MedrunnerApiClient.Models;

/// <summary>
/// Enum for account deactivation reasons.
/// </summary>
public enum AccountDeactivationReason
{
    NONE,
    CLIENT_DRIVEN_DELETION,
    TERMINATED,
    BLOCKED
}
