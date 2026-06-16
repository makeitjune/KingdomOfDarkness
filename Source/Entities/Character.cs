using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using KingdomOfDarkness.Core;

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

    public override void Draw(SpriteBatch spriteBatch, Camera2D camera)
    {
        if (IsDead) return;

        // Visual size of the character placeholder (e.g. 24px wide, 48px high)
        int charWidth = 24;
        int charHeight = 48;

        // Get screen position from world position (which corresponds to the feet/base)
        Vector2 screenPos = camera.WorldToCameraScreen(WorldPosition);

        // Draw character rectangle with feet origin (bottom-center)
        // originX = width / 2, originY = height
        Vector2 origin = new Vector2(charWidth / 2f, charHeight);

        Rectangle destRect = new Rectangle(
            (int)screenPos.X,
            (int)screenPos.Y,
            charWidth,
            charHeight
        );

        spriteBatch.Draw(
            WhitePixelTexture,
            destRect,
            null,
            DebugColor,
            0f,
            origin,
            SpriteEffects.None,
            0f
        );

        // Draw a small nameplate or health bar outline above the character
        // We will build a more detailed HUD/HealthBar system in Phase 7, 
        // but it's nice to draw a simple green line for HP here.
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
