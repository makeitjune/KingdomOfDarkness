using Microsoft.Xna.Framework;

namespace KingdomOfDarkness.Core;

public static class IsoMath
{
    /// <summary>
    /// Converts a logical world position to screen pixel position.
    /// </summary>
    public static Vector2 WorldToScreen(Vector2 world)
    {
        float screenX = (world.X - world.Y) * GameConstants.TileWidth / 2f;
        float screenY = (world.X + world.Y) * GameConstants.TileHeight / 2f;
        return new Vector2(screenX, screenY);
    }

    /// <summary>
    /// Converts a screen pixel position to an approximate logical world position.
    /// Note: Does not account for camera offset. Apply camera shift before calling this.
    /// </summary>
    public static Vector2 ScreenToWorldApprox(Vector2 screen)
    {
        float worldX = (screen.Y / GameConstants.TileHeight) + (screen.X / GameConstants.TileWidth);
        float worldY = (screen.Y / GameConstants.TileHeight) - (screen.X / GameConstants.TileWidth);
        return new Vector2(worldX, worldY);
    }
}
