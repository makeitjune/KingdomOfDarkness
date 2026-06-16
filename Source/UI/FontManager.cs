using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace KingdomOfDarkness.UI;

/// <summary>
/// Manages SpriteFont loading and provides convenient text drawing methods.
/// Replaces SimpleFont for all text rendering, supporting Korean (Hangul) characters.
/// </summary>
public static class FontManager
{
    private static SpriteFont _gameFont;

    /// <summary>
    /// Whether the font has been successfully loaded.
    /// </summary>
    public static bool IsLoaded => _gameFont != null;

    /// <summary>
    /// Loads the game font from the content pipeline.
    /// Call this once in Game1.LoadContent().
    /// </summary>
    public static void LoadContent(ContentManager content)
    {
        _gameFont = content.Load<SpriteFont>("GameFont");
    }

    /// <summary>
    /// Draws a string at the given position with the specified color and scale.
    /// </summary>
    public static void DrawString(SpriteBatch spriteBatch, string text, Vector2 position, Color color, float scale = 1.0f)
    {
        if (!IsLoaded || string.IsNullOrEmpty(text)) return;
        spriteBatch.DrawString(_gameFont, text, position, color, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
    }

    /// <summary>
    /// Draws a string centered at the given position.
    /// </summary>
    public static void DrawStringCentered(SpriteBatch spriteBatch, string text, Vector2 centerPosition, Color color, float scale = 1.0f)
    {
        if (!IsLoaded || string.IsNullOrEmpty(text)) return;
        Vector2 size = MeasureString(text, scale);
        Vector2 pos = centerPosition - size / 2f;
        spriteBatch.DrawString(_gameFont, text, pos, color, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
    }

    /// <summary>
    /// Measures the size of a string when drawn at the given scale.
    /// </summary>
    public static Vector2 MeasureString(string text, float scale = 1.0f)
    {
        if (!IsLoaded || string.IsNullOrEmpty(text)) return Vector2.Zero;
        return _gameFont.MeasureString(text) * scale;
    }

    /// <summary>
    /// Gets the underlying SpriteFont for direct usage if needed.
    /// </summary>
    public static SpriteFont GetFont()
    {
        return _gameFont;
    }
}
