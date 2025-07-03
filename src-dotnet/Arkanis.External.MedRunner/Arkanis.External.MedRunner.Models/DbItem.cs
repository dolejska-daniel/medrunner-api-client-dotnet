namespace Arkanis.External.MedRunner.Models;

/// <summary>
///     Base class for DB items.
/// </summary>
public class DbItem
{
    /// <summary>
    ///     The unique identifier for the item.
    /// </summary>
    public required string Id { get; set; }

    /// <summary>
    ///     The creation timestamp.
    /// </summary>
    public required string Created { get; set; }

    /// <summary>
    ///     The last updated timestamp.
    /// </summary>
    public required string Updated { get; set; }
}
