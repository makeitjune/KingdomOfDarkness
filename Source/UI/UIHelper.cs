using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KingdomOfDarkness.UI;

public static class UIHelper
{
    private static Texture2D _pixel;

    public static void Initialize(Texture2D pixel)
    {
        _pixel = pixel;
    }

    public static void DrawWoodBackground(SpriteBatch spriteBatch, Rectangle rect)
    {
        // Base dark brown
        spriteBatch.Draw(_pixel, rect, new Color(90, 60, 40));
        
        // Add horizontal wood grains
        for (int i = 0; i < rect.Height; i += 2)
        {
            float alpha = (i % 4 == 0) ? 0.2f : 0.05f;
            spriteBatch.Draw(_pixel, new Rectangle(rect.X, rect.Y + i, rect.Width, 1), new Color(0, 0, 0, alpha));
        }
    }

    public static void DrawGoldFrame(SpriteBatch spriteBatch, Rectangle rect, int bw)
    {
        Color borderTopLeft = new Color(212, 175, 55); 
        Color borderBottomRight = new Color(139, 101, 8); 
        Color borderOuter = new Color(20, 10, 5); 

        // Outer shadow edge
        spriteBatch.Draw(_pixel, new Rectangle(rect.X - 1, rect.Y - 1, rect.Width + 2, 1), borderOuter);
        spriteBatch.Draw(_pixel, new Rectangle(rect.X - 1, rect.Bottom, rect.Width + 2, 1), borderOuter);
        spriteBatch.Draw(_pixel, new Rectangle(rect.X - 1, rect.Y - 1, 1, rect.Height + 2), borderOuter);
        spriteBatch.Draw(_pixel, new Rectangle(rect.Right, rect.Y - 1, 1, rect.Height + 2), borderOuter);

        // Bright top/left
        spriteBatch.Draw(_pixel, new Rectangle(rect.X, rect.Y, rect.Width, bw), borderTopLeft);
        spriteBatch.Draw(_pixel, new Rectangle(rect.X, rect.Y, bw, rect.Height), borderTopLeft);

        // Dark bottom/right
        spriteBatch.Draw(_pixel, new Rectangle(rect.X, rect.Bottom - bw, rect.Width, bw), borderBottomRight);
        spriteBatch.Draw(_pixel, new Rectangle(rect.Right - bw, rect.Y, bw, rect.Height), borderBottomRight);
        
        // Inner thin line
        spriteBatch.Draw(_pixel, new Rectangle(rect.X + bw, rect.Y + bw, rect.Width - bw*2, 1), new Color(255, 220, 100));
        spriteBatch.Draw(_pixel, new Rectangle(rect.X + bw, rect.Y + bw, 1, rect.Height - bw*2), new Color(255, 220, 100));
        spriteBatch.Draw(_pixel, new Rectangle(rect.X + bw, rect.Bottom - bw - 1, rect.Width - bw*2, 1), new Color(80, 50, 10));
        spriteBatch.Draw(_pixel, new Rectangle(rect.Right - bw - 1, rect.Y + bw, 1, rect.Height - bw*2), new Color(80, 50, 10));
    }

    public static void DrawRetroPanel(SpriteBatch spriteBatch, Rectangle rect)
    {
        DrawWoodBackground(spriteBatch, rect);
        DrawGoldFrame(spriteBatch, rect, 3);
    }

    public static void DrawInsetBox(SpriteBatch spriteBatch, Rectangle rect)
    {
        // Deep dark background
        Color insetBase = new Color(25, 18, 15);
        spriteBatch.Draw(_pixel, rect, insetBase);

        // Shading
        Color darkShadow = new Color(5, 2, 0);
        Color brightHighlight = new Color(130, 95, 70);

        int bw = 1;
        spriteBatch.Draw(_pixel, new Rectangle(rect.X, rect.Y, rect.Width, bw), darkShadow); // Top
        spriteBatch.Draw(_pixel, new Rectangle(rect.X, rect.Y, bw, rect.Height), darkShadow); // Left
        spriteBatch.Draw(_pixel, new Rectangle(rect.X, rect.Bottom - bw, rect.Width, bw), brightHighlight); // Bottom
        spriteBatch.Draw(_pixel, new Rectangle(rect.Right - bw, rect.Y, bw, rect.Height), brightHighlight); // Right
    }

    public static void DrawHpMpOrb(SpriteBatch spriteBatch, Rectangle rect, float percentage, Color color)
    {
        // Dark pill background
        spriteBatch.Draw(_pixel, rect, new Color(10, 10, 10));
        
        // Fill rect
        int fillHeight = (int)((rect.Height - 4) * percentage);
        if (fillHeight < 0) fillHeight = 0;
        
        Rectangle fillRect = new Rectangle(rect.X + 2, rect.Bottom - 2 - fillHeight, rect.Width - 4, fillHeight);
        spriteBatch.Draw(_pixel, fillRect, color);

        // Highlight (Left side bright, right side dark)
        spriteBatch.Draw(_pixel, new Rectangle(fillRect.X, fillRect.Y, 4, fillRect.Height), new Color(255, 255, 255, 80));
        spriteBatch.Draw(_pixel, new Rectangle(fillRect.Right - 4, fillRect.Y, 4, fillRect.Height), new Color(0, 0, 0, 80));

        // Bubbles (horizontal lines)
        int segmentCount = 7;
        float segmentHeight = (rect.Height - 4) / (float)segmentCount;
        for (int i = 1; i < segmentCount; i++)
        {
            int y = rect.Y + 2 + (int)(i * segmentHeight);
            spriteBatch.Draw(_pixel, new Rectangle(rect.X + 2, y - 1, rect.Width - 4, 2), new Color(0, 0, 0, 150));
            // White shine bubble
            spriteBatch.Draw(_pixel, new Rectangle(rect.X + rect.Width / 2 - 2, y - 4, 4, 3), new Color(255, 255, 255, 180));
        }
        
        // Frame
        DrawGoldFrame(spriteBatch, rect, 2);
    }
}
