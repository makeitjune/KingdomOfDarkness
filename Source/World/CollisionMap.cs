using Microsoft.Xna.Framework;

namespace KingdomOfDarkness.World;

public class CollisionMap
{
    private readonly IsoTileMap _tileMap;

    public CollisionMap(IsoTileMap tileMap)
    {
        _tileMap = tileMap;
    }

    /// <summary>
    /// Checks if the logical world coordinate is blocked or out of map boundaries.
    /// </summary>
    public bool IsBlocked(float worldX, float worldY)
    {
        int tileX = (int)System.Math.Round(worldX);
        int tileY = (int)System.Math.Round(worldY);

        // Boundary check
        if (tileX < 0 || tileX >= _tileMap.Width || tileY < 0 || tileY >= _tileMap.Height)
        {
            return true; // Out of bounds is blocked
        }

        IsoTile tile = _tileMap.GetTile(tileX, tileY);
        return tile != null && tile.IsBlocked;
    }

    /// <summary>
    /// Helper to convert a float world coordinate to its corresponding integer tile grid coordinate.
    /// </summary>
    public Point WorldToTile(Vector2 worldPosition)
    {
        return new Point(
            (int)System.Math.Round(worldPosition.X),
            (int)System.Math.Round(worldPosition.Y)
        );
    }
}
