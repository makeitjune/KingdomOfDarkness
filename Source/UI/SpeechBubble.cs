using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using KingdomOfDarkness.Core;
using KingdomOfDarkness.Entities;

namespace KingdomOfDarkness.UI;

public class SpeechBubble
{
    public string Text { get; set; } = string.Empty;
    public float RemainingSeconds { get; set; } = 0f;
    public bool IsActive => RemainingSeconds > 0f;

    public void Show(string text, float durationSeconds = 3.0f)
    {
        Text = text;
        RemainingSeconds = durationSeconds;
    }

    public void Update(GameTime gameTime)
    {
        if (RemainingSeconds > 0f)
        {
            RemainingSeconds -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (RemainingSeconds < 0f)
                RemainingSeconds = 0f;
        }
    }

    public void Draw(SpriteBatch spriteBatch, Camera2D camera, Character character, Texture2D whitePixel)
    {
        if (!IsActive || string.IsNullOrEmpty(Text)) return;

        // Position bubble above the character's head
        // The character's WorldPosition is at their feet
        int charHeight = 48; 
        Vector2 feetScreenPos = camera.WorldToCameraScreen(character.WorldPosition);

        // Measure text size
        float fontScale = 0.9f;
        Vector2 textSize = SimpleFont.MeasureString(Text, fontScale);

        // Bubble sizes with padding
        int paddingX = 8;
        int paddingY = 6;
        int bubbleW = (int)textSize.X + paddingX * 2;
        int bubbleH = (int)textSize.Y + paddingY * 2;

        // Centered horizontally above head
        Vector2 bubblePos = feetScreenPos - new Vector2(bubbleW / 2f, charHeight + bubbleH + 15f);

        // 1. Draw Bubble Box Background (Semi-transparent black/grey for Sleek Dark Theme)
        Rectangle bgRect = new Rectangle((int)bubblePos.X, (int)bubblePos.Y, bubbleW, bubbleH);
        spriteBatch.Draw(
            whitePixel,
            bgRect,
            new Color(15, 18, 22, 220) // Deep rich dark glassmorphism
        );

        // 2. Draw Bubble Border (Thin light gray outline)
        DrawOutline(spriteBatch, whitePixel, bgRect, new Color(120, 130, 140, 180), 1);

        // 3. Draw Speech Bubble Pointer Triangle (Centered at bottom of bubble pointing to character)
        Vector2 pointerPos = new Vector2(feetScreenPos.X - 4, bubblePos.Y + bubbleH);
        spriteBatch.Draw(
            whitePixel,
            new Rectangle((int)pointerPos.X, (int)pointerPos.Y, 8, 4),
            new Color(15, 18, 22, 220)
        );
        // Border for pointer (left/right lines)
        DrawLine(spriteBatch, whitePixel, new Vector2(pointerPos.X, pointerPos.Y), new Vector2(pointerPos.X + 4, pointerPos.Y + 4), new Color(120, 130, 140, 180), 1);
        DrawLine(spriteBatch, whitePixel, new Vector2(pointerPos.X + 8, pointerPos.Y), new Vector2(pointerPos.X + 4, pointerPos.Y + 4), new Color(120, 130, 140, 180), 1);

        // 4. Draw Text centered inside
        Vector2 textPos = bubblePos + new Vector2(paddingX, paddingY);
        SimpleFont.DrawString(spriteBatch, whitePixel, Text, textPos, Color.White, fontScale);
    }

    private void DrawOutline(SpriteBatch sb, Texture2D pixel, Rectangle rect, Color color, int thickness)
    {
        // Top
        sb.Draw(pixel, new Rectangle(rect.X, rect.Y, rect.Width, thickness), color);
        // Bottom
        sb.Draw(pixel, new Rectangle(rect.X, rect.Y + rect.Height - thickness, rect.Width, thickness), color);
        // Left
        sb.Draw(pixel, new Rectangle(rect.X, rect.Y, thickness, rect.Height), color);
        // Right
        sb.Draw(pixel, new Rectangle(rect.X + rect.Width - thickness, rect.Y, thickness, rect.Height), color);
    }

    private void DrawLine(SpriteBatch sb, Texture2D pixel, Vector2 p1, Vector2 p2, Color color, float thickness)
    {
        float angle = (float)Math.Atan2(p2.Y - p1.Y, p2.X - p1.X);
        float length = Vector2.Distance(p1, p2);
        sb.Draw(pixel, p1, null, color, angle, Vector2.Zero, new Vector2(length, thickness), SpriteEffects.None, 0f);
    }
}
