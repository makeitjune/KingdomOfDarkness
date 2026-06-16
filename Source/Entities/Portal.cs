using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using KingdomOfDarkness.Core;
using KingdomOfDarkness.UI;

namespace KingdomOfDarkness.Entities;

public class Portal : Entity
{
    private readonly Texture2D _texture;
    private readonly Color _color;

    public MapType TargetMapType { get; private set; }
    public Vector2 TargetSpawnPosition { get; private set; }

    public Portal(Texture2D texture, Vector2 position, MapType targetMapType, Vector2 targetSpawnPosition, Color color)
    {
        _texture = texture;
        WorldPosition = position;
        TargetMapType = targetMapType;
        TargetSpawnPosition = targetSpawnPosition;
        _color = color;
    }

    public override void Update(GameTime gameTime)
    {
        // Portals don't need update logic right now
    }

    public override void Draw(SpriteBatch spriteBatch, Camera2D camera)
    {
        Vector2 screenPos = camera.WorldToCameraScreen(WorldPosition);
        
        // Draw portal as a wide ellipse/diamond
        int width = GameConstants.TileWidth;
        int height = GameConstants.TileHeight;

        Rectangle rect = new Rectangle(
            (int)screenPos.X - width / 2,
            (int)screenPos.Y - height / 2,
            width,
            height
        );

        // Simple representation: semi-transparent colored block covering the tile
        spriteBatch.Draw(_texture, rect, _color * 0.5f);
        
        // "Portal" text
        Vector2 textSize = FontManager.MeasureString("Portal", 0.6f);
        FontManager.DrawString(spriteBatch, "Portal", screenPos - new Vector2(textSize.X / 2, height / 2 + 10), Color.White, 0.6f);
    }
}
