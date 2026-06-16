using Microsoft.Xna.Framework;

namespace KingdomOfDarkness.World;

public enum TileType
{
    Grass,
    Dirt,
    Stone,
    Sand,
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

    public IsoTile(int x, int y, TileType type = TileType.Grass)
    {
        X = x;
        Y = y;
        Type = type;
        
        // Setup initial default color based on type
        switch (Type)
        {
            case TileType.Water:
                DebugColor = new Color(50, 100, 200); // Blueish water
                break;
            case TileType.Blocked:
                DebugColor = new Color(100, 100, 100); // Gray stone wall
                break;
            case TileType.Dirt:
                DebugColor = new Color(120, 80, 50); // Brown dirt
                break;
            case TileType.Stone:
                DebugColor = new Color(150, 150, 150); // Light gray stone path
                break;
            case TileType.Sand:
                DebugColor = new Color(200, 180, 120); // Yellowish sand
                break;
            case TileType.Grass:
            default:
                // Checkerboard pattern for grass ground
                bool isEven = (x + y) % 2 == 0;
                DebugColor = isEven ? new Color(60, 120, 60) : new Color(50, 110, 50);
                break;
        }
    }
}
