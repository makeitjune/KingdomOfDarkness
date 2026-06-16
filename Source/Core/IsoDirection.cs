namespace KingdomOfDarkness.Core;

/// <summary>
/// Represents the 4 diagonal facing directions in quarter-view space.
/// These correspond to the visual screen directions produced by isometric projection.
/// </summary>
public enum IsoDirection
{
    /// <summary>Screen down-right (worldX+ direction)</summary>
    SouthEast,
    /// <summary>Screen down-left (worldY+ direction)</summary>
    SouthWest,
    /// <summary>Screen up-left (worldX- direction)</summary>
    NorthWest,
    /// <summary>Screen up-right (worldY- direction)</summary>
    NorthEast
}
