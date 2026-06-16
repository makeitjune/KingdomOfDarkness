using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using KingdomOfDarkness.Core;

namespace KingdomOfDarkness.UI;

/// <summary>
/// A single floating damage number that rises and fades out.
/// </summary>
public class DamagePopup
{
    public Vector2 WorldPosition { get; set; }
    public int DamageValue { get; set; }
    public Color TextColor { get; set; }
    public float RemainingSeconds { get; set; }
    public float TotalDuration { get; set; }
    public float RiseSpeed { get; set; } = 30f; // pixels per second rising
    public bool IsActive => RemainingSeconds > 0f;

    // Screen offset accumulated over time (for the rising animation)
    private float _riseOffset;

    public DamagePopup(Vector2 worldPosition, int damage, Color color, float duration = 1.2f)
    {
        WorldPosition = worldPosition;
        DamageValue = damage;
        TextColor = color;
        RemainingSeconds = duration;
        TotalDuration = duration;
        _riseOffset = 0f;
    }

    public void Update(GameTime gameTime)
    {
        if (!IsActive) return;

        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
        RemainingSeconds -= dt;
        _riseOffset += RiseSpeed * dt;

        if (RemainingSeconds < 0f)
            RemainingSeconds = 0f;
    }

    public void Draw(SpriteBatch spriteBatch, Camera2D camera)
    {
        if (!IsActive) return;

        // Calculate fade-out alpha
        float lifeRatio = RemainingSeconds / TotalDuration;
        float alpha = MathHelper.Clamp(lifeRatio * 2f, 0f, 1f); // fade in last 50%

        // Screen position (above the world position, rising up)
        Vector2 screenPos = camera.WorldToCameraScreen(WorldPosition);
        screenPos.Y -= 50f + _riseOffset; // start above character head

        string text = DamageValue.ToString();
        Color drawColor = TextColor * alpha;
        float scale = 0.8f;

        // Draw with slight outline/shadow for readability
        Vector2 textSize = FontManager.MeasureString(text, scale);
        Vector2 drawPos = new Vector2(screenPos.X - textSize.X / 2f, screenPos.Y);

        // Shadow
        FontManager.DrawString(spriteBatch, text, drawPos + new Vector2(1, 1), Color.Black * alpha * 0.6f, scale);
        // Main text
        FontManager.DrawString(spriteBatch, text, drawPos, drawColor, scale);
    }
}
