namespace Arkanis.External.MedRunner.Models;

/// <summary>
///     Enum for user roles.
/// </summary>
public enum UserRoles
{
    CLIENT = 1 << 0,
    STAFF = 1 << 1,
    DEVELOPER = 1 << 51,
    BOT = 1 << 52,
}
