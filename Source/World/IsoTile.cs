using Microsoft.Xna.Framework;

namespace KingdomOfDarkness.World;

public enum TileType
{
    Ground,
    Blocked,
    Water
}

public class IsoTile
{
    public int X { get; }
    public int Y { get; }
    public TileType Type { get; set; }
    public bool IsBlocked => Type == TileType.Blocked || Type == TileType.Water;
    public Color DebugColor { get; set; }

    public IsoTile(int x, int y, TileType type = TileType.Ground)
    {
        X = x;
        Y = y;
        Type = type;
        
        // Default color variation to look nice
        if (type == TileType.Blocked)
            DebugColor = new Color(70, 70, 70); // Dark gray
        else if (type == TileType.Water)
            DebugColor = new Color(30, 80, 150); // Deep blue
        else
            // Checkerboard color pattern for ground
            DebugColor = ((x + y) % 2 == 0) ? new Color(34, 139, 34) : new Color(46, 139, 87); // Forest green / Sea green
    }
}
