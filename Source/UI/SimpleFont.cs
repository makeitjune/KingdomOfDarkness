using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KingdomOfDarkness.UI;

public static class SimpleFont
{
    private static void DrawLine(SpriteBatch sb, Texture2D pixel, Vector2 p1, Vector2 p2, Color color, float thickness)
    {
        float angle = (float)Math.Atan2(p2.Y - p1.Y, p2.X - p1.X);
        float length = Vector2.Distance(p1, p2);
        sb.Draw(
            pixel,
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

    public static void DrawChar(SpriteBatch sb, Texture2D pixel, char c, Vector2 pos, float w, float h, Color color, float thickness)
    {
        // Define 9 grid points:
        // 0 (TL)  1 (TM)  2 (TR)
        // 3 (ML)  4 (MM)  5 (MR)
        // 6 (BL)  7 (BM)  8 (BR)
        Vector2[] p = new Vector2[9];
        p[0] = pos;
        p[1] = pos + new Vector2(w / 2f, 0f);
        p[2] = pos + new Vector2(w, 0f);
        p[3] = pos + new Vector2(0f, h / 2f);
        p[4] = pos + new Vector2(w / 2f, h / 2f);
        p[5] = pos + new Vector2(w, h / 2f);
        p[6] = pos + new Vector2(0f, h);
        p[7] = pos + new Vector2(w / 2f, h);
        p[8] = pos + new Vector2(w, h);

        Action<int, int> L = (i1, i2) => DrawLine(sb, pixel, p[i1], p[i2], color, thickness);

        switch (char.ToUpper(c))
        {
            case 'A': L(6, 0); L(0, 2); L(2, 8); L(3, 5); break;
            case 'B': L(6, 0); L(0, 2); L(2, 5); L(5, 3); L(5, 8); L(8, 6); break;
            case 'C': L(2, 0); L(0, 6); L(6, 8); break;
            case 'D': L(0, 6); L(0, 2); L(2, 8); L(8, 6); break;
            case 'E': L(2, 0); L(0, 6); L(6, 8); L(3, 4); break;
            case 'F': L(2, 0); L(0, 6); L(3, 4); break;
            case 'G': L(2, 0); L(0, 6); L(6, 8); L(8, 5); L(5, 4); break;
            case 'H': L(0, 6); L(2, 8); L(3, 5); break;
            case 'I': L(0, 2); L(1, 7); L(6, 8); break;
            case 'J': L(2, 8); L(8, 6); L(6, 3); break;
            case 'K': L(0, 6); L(3, 2); L(3, 8); break;
            case 'L': L(0, 6); L(6, 8); break;
            case 'M': L(6, 0); L(0, 4); L(4, 2); L(2, 8); break;
            case 'N': L(6, 0); L(0, 8); L(8, 2); break;
            case 'O': L(0, 2); L(2, 8); L(8, 6); L(6, 0); break;
            case 'P': L(6, 0); L(0, 2); L(2, 5); L(5, 3); break;
            case 'Q': L(0, 2); L(2, 8); L(8, 6); L(6, 0); L(4, 8); break;
            case 'R': L(6, 0); L(0, 2); L(2, 5); L(5, 3); L(3, 8); break;
            case 'S': L(2, 0); L(0, 3); L(3, 5); L(5, 8); L(8, 6); break;
            case 'T': L(0, 2); L(1, 7); break;
            case 'U': L(0, 6); L(6, 8); L(8, 2); break;
            case 'V': L(0, 7); L(7, 2); break;
            case 'W': L(0, 6); L(6, 4); L(4, 8); L(8, 2); break;
            case 'X': L(0, 8); L(2, 6); break;
            case 'Y': L(0, 4); L(2, 4); L(4, 7); break;
            case 'Z': L(0, 2); L(2, 6); L(6, 8); break;
            case '0': L(0, 2); L(2, 8); L(8, 6); L(6, 0); L(2, 6); break; // crossed zero
            case '1': L(0, 1); L(1, 7); L(6, 8); break;
            case '2': L(0, 2); L(2, 5); L(5, 6); L(6, 8); break;
            case '3': L(0, 2); L(2, 8); L(8, 6); L(3, 5); break;
            case '4': L(0, 3); L(3, 5); L(2, 8); break;
            case '5': L(2, 0); L(0, 3); L(3, 5); L(5, 8); L(8, 6); break;
            case '6': L(2, 0); L(0, 6); L(6, 8); L(8, 5); L(5, 3); break;
            case '7': L(0, 2); L(2, 8); break;
            case '8': L(0, 2); L(2, 8); L(8, 6); L(6, 0); L(3, 5); break;
            case '9': L(6, 8); L(8, 2); L(2, 0); L(0, 3); L(3, 5); break;
            case '.': L(7, 7); break;
            case ':': L(1, 1); L(7, 7); break;
            case '!': L(1, 4); L(7, 7); break;
            case ',': L(7, 6); break;
            case '-': L(3, 5); break;
            case '/': L(6, 2); break;
            case '\'': L(1, 0); break;
            default: break; // Space or unsupported char
        }
    }

    public static void DrawString(SpriteBatch sb, Texture2D pixel, string text, Vector2 position, Color color, float scale = 1.0f)
    {
        if (string.IsNullOrEmpty(text)) return;

        float charW = 8f;
        float charH = 12f;
        float spacing = 3f;
        float thickness = 1.5f * scale;

        Vector2 currentPos = position;

        for (int i = 0; i < text.Length; i++)
        {
            char c = text[i];
            if (c == '\n')
            {
                currentPos.X = position.X;
                currentPos.Y += (charH + 5f) * scale;
                continue;
            }

            DrawChar(sb, pixel, c, currentPos, charW * scale, charH * scale, color, thickness);
            currentPos.X += (charW + spacing) * scale;
        }
    }

    public static Vector2 MeasureString(string text, float scale = 1.0f)
    {
        if (string.IsNullOrEmpty(text)) return Vector2.Zero;

        float charW = 8f;
        float charH = 12f;
        float spacing = 3f;

        float maxLineWidth = 0f;
        float currentLineWidth = 0f;
        int linesCount = 1;

        for (int i = 0; i < text.Length; i++)
        {
            char c = text[i];
            if (c == '\n')
            {
                maxLineWidth = Math.Max(maxLineWidth, currentLineWidth);
                currentLineWidth = 0f;
                linesCount++;
                continue;
            }

            currentLineWidth += (charW + spacing) * scale;
        }

        maxLineWidth = Math.Max(maxLineWidth, currentLineWidth);
        
        // Remove trailing spacing from measurement
        if (maxLineWidth > 0f)
            maxLineWidth -= spacing * scale;

        float totalHeight = (charH * linesCount + (linesCount - 1) * 5f) * scale;
        return new Vector2(maxLineWidth, totalHeight);
    }
}
