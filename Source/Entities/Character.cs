using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using KingdomOfDarkness.Core;
using KingdomOfDarkness.UI;

namespace KingdomOfDarkness.Entities;

public abstract class Character : Entity
{
    public string Name { get; set; }
    
    // Stats
    public int Level { get; set; } = 1;
    public int CurrentHP { get; set; }
    public int MaxHP { get; set; }
    public int AttackPower { get; set; }
    public int Defense { get; set; }
    public float AttackRange { get; set; } = 1.2f; // world units
    public float AttackCooldownSeconds { get; set; } = 1.0f;
    public float AttackCooldownRemaining { get; set; } = 0.0f;
    public int Experience { get; set; } = 0;
    
    public bool IsDead => CurrentHP <= 0;

    // Movement
    public Vector2 Velocity { get; set; }
    public float MoveSpeed { get; set; }

    // Rendering Placeholder
    public Color DebugColor { get; set; } = Color.White;
    protected Texture2D WhitePixelTexture { get; set; }
    
    // For combat targeting
    public Character Target { get; set; }
    public bool IsTargeted { get; set; } = false;

    protected Character(Texture2D whitePixel, string name, int maxHp, int attackPower, int defense, float moveSpeed)
    {
        WhitePixelTexture = whitePixel;
        Name = name;
        MaxHP = maxHp;
        CurrentHP = maxHp;
        AttackPower = attackPower;
        Defense = defense;
        MoveSpeed = moveSpeed;
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        // Tick attack cooldown
        if (AttackCooldownRemaining > 0f)
        {
            AttackCooldownRemaining -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (AttackCooldownRemaining < 0f)
                AttackCooldownRemaining = 0f;
        }
    }

    private void DrawLine(SpriteBatch sb, Vector2 p1, Vector2 p2, Color color, float thickness = 1.5f)
    {
        float angle = (float)System.Math.Atan2(p2.Y - p1.Y, p2.X - p1.X);
        float length = Vector2.Distance(p1, p2);
        sb.Draw(
            WhitePixelTexture,
            p1,
            null,
            color,
            angle,
            Vector2.Zero,
            new Vector2(length, thickness),
            SpriteEffects.None,
            0f
        );
    }

    private void DrawTargetMarker(SpriteBatch sb, Vector2 center)
    {
        // Draw red selection diamond around character feet
        float halfW = 20f;
        float halfH = 10f;
        
        Vector2 top = center - new Vector2(0, halfH);
        Vector2 bottom = center + new Vector2(0, halfH);
        Vector2 left = center - new Vector2(halfW, 0);
        Vector2 right = center + new Vector2(halfW, 0);

        Color targetColor = Color.Red;
        float thickness = 2.0f;

        DrawLine(sb, top, right, targetColor, thickness);
        DrawLine(sb, right, bottom, targetColor, thickness);
        DrawLine(sb, bottom, left, targetColor, thickness);
        DrawLine(sb, left, top, targetColor, thickness);
    }

    public override void Draw(SpriteBatch spriteBatch, Camera2D camera)
    {
        if (IsDead) return;

        // Visual size of the character placeholder (e.g. 24px wide, 48px high)
        int charWidth = 24;
        int charHeight = 48;

        // Get screen position from world position (which corresponds to the feet/base)
        Vector2 screenPos = camera.WorldToCameraScreen(WorldPosition);

        // 1. Draw simple shadow under feet (semi-transparent black)
        Rectangle shadowRect = new Rectangle(
            (int)(screenPos.X - 16),
            (int)(screenPos.Y - 6),
            32,
            12
        );
        spriteBatch.Draw(
            WhitePixelTexture,
            shadowRect,
            new Color(0, 0, 0, 100) // 100 alpha shadow
        );

        // 2. Draw targeted selection marker if applicable
        if (IsTargeted)
        {
            DrawTargetMarker(spriteBatch, screenPos);
        }

        // 3. Draw character rectangle offset manually to avoid large origin scaling on 1x1 texture
        Rectangle destRect = new Rectangle(
            (int)(screenPos.X - charWidth / 2f),
            (int)(screenPos.Y - charHeight),
            charWidth,
            charHeight
        );

        spriteBatch.Draw(
            WhitePixelTexture,
            destRect,
            DebugColor
        );

        // 4. Draw name label above character
        string nameUpper = Name.ToUpper();
        float nameScale = 0.7f;
        Vector2 nameSize = SimpleFont.MeasureString(nameUpper, nameScale);
        Vector2 namePos = new Vector2(screenPos.X - nameSize.X / 2f, screenPos.Y - charHeight - 24f);
        
        Color labelColor = DebugColor;
        if (labelColor == Color.White) labelColor = Color.LightGray;
        SimpleFont.DrawString(spriteBatch, WhitePixelTexture, nameUpper, namePos, labelColor, nameScale);

        // 5. Draw status health bar
        DrawSimpleHealthBar(spriteBatch, screenPos, charHeight);
    }

    private void DrawSimpleHealthBar(SpriteBatch spriteBatch, Vector2 feetScreenPos, int charHeight)
    {
        if (MaxHP <= 0) return;

        int barWidth = 32;
        int barHeight = 4;
        // Positioned above the head of the character
        Vector2 barPos = feetScreenPos - new Vector2(barWidth / 2f, charHeight + 10f);

        // Background (Red)
        spriteBatch.Draw(
            WhitePixelTexture,
            new Rectangle((int)barPos.X, (int)barPos.Y, barWidth, barHeight),
            Color.Red
        );

        // Foreground (Green)
        float hpPercent = (float)CurrentHP / MaxHP;
        int fillWidth = (int)(barWidth * MathHelper.Clamp(hpPercent, 0f, 1f));

        spriteBatch.Draw(
            WhitePixelTexture,
            new Rectangle((int)barPos.X, (int)barPos.Y, fillWidth, barHeight),
            Color.Green
        );
    }
}
