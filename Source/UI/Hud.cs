using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using KingdomOfDarkness.Core;
using KingdomOfDarkness.Entities;

namespace KingdomOfDarkness.UI;

public class Hud
{
    private readonly Texture2D _whitePixel;
    public const int PanelHeight = 144;
    public const int PanelY = GameConstants.VirtualHeight - PanelHeight; // 600 - 144 = 456

    public Hud(Texture2D whitePixel)
    {
        _whitePixel = whitePixel;
    }

    public void Draw(SpriteBatch spriteBatch, Player player, List<Companion> companions, Texture2D whitePixel)
    {
        int logicalWidth = GameConstants.VirtualWidth;
        int logicalHeight = GameConstants.VirtualHeight;
        int hudY = PanelY;
        
        // Draw Full Width Background
        Rectangle bottomRect = new Rectangle(0, hudY, logicalWidth, PanelHeight);
        UIHelper.DrawRetroPanel(spriteBatch, bottomRect);

        // Define panel boundaries
        int leftWidth = 120;
        int rightWidth = 280; // 800 - 520 = 280
        int cX = leftWidth;
        int rX = logicalWidth - rightWidth;

        float labelFontScale = 0.85f;
        float valueFontScale = 1.0f;

        // ==========================================
        // LEFT PANEL (0 to 120)
        // ==========================================
        int orbW = 30;
        int orbH = 90;
        int orbY = hudY + 30;

        // HP Text
        UIHelper.DrawInsetBox(spriteBatch, new Rectangle(10, hudY + 10, 100, 16));
        DrawTextRightAligned(spriteBatch, player.CurrentHP.ToString(), 105, hudY + 10, new Color(255, 100, 100), valueFontScale);
        
        // HP Orb
        Rectangle hpRect = new Rectangle(15, orbY, orbW, orbH);
        float hpRatio = player.MaxHP > 0 ? (float)player.CurrentHP / player.MaxHP : 0f;
        UIHelper.DrawHpMpOrb(spriteBatch, hpRect, hpRatio, new Color(220, 30, 30));

        // MP Orb
        Rectangle mpRect = new Rectangle(75, orbY, orbW, orbH);
        float mpRatio = player.MaxMP > 0 ? (float)player.CurrentMP / player.MaxMP : 0f;
        UIHelper.DrawHpMpOrb(spriteBatch, mpRect, mpRatio, new Color(30, 100, 240));

        // MP Text
        int mpTextY = orbY + orbH + 5;
        UIHelper.DrawInsetBox(spriteBatch, new Rectangle(10, mpTextY, 100, 16));
        DrawTextRightAligned(spriteBatch, player.CurrentMP.ToString(), 105, mpTextY, new Color(100, 150, 255), valueFontScale);

        // ==========================================
        // CENTER PANEL (120 to 520) Width: 400
        // ==========================================
        // ==========================================
        // CENTER PANEL (120 to 520) Width: 400
        // ==========================================
        int msgY = hudY + 10;
        int msgH = 22;
        UIHelper.DrawInsetBox(spriteBatch, new Rectangle(cX, msgY, 390, msgH));
        DrawText(spriteBatch, "Welcome.", cX + 10, msgY + 3, new Color(220, 180, 120), labelFontScale);

        // Stat Area
        int statStartX = cX + 10;
        int sY = msgY + msgH + 10;
        int rowH = 18;

        // Col 1
        string[] stats = { "STR", "INT", "WIS", "CON", "DEX" };
        int[] statVals = { 5, 97, 109, 4, 3 }; 
        for (int i = 0; i < 5; i++)
        {
            int y = sY + (i * rowH);
            DrawText(spriteBatch, stats[i], statStartX, y, new Color(180, 160, 140), labelFontScale);
            UIHelper.DrawInsetBox(spriteBatch, new Rectangle(statStartX + 35, y + 1, 30, 16));
            DrawTextRightAligned(spriteBatch, statVals[i].ToString(), statStartX + 60, y, Color.Wheat, valueFontScale);
            spriteBatch.Draw(_whitePixel, new Rectangle(statStartX + 70, y + 3, 10, 10), new Color(100, 80, 60));
        }

        // Col 2
        int c2X = statStartX + 90;
        int col2LblW = 45;
        DrawText(spriteBatch, "HP", c2X, sY, new Color(180, 160, 140), labelFontScale);
        DrawText(spriteBatch, "MP", c2X, sY + rowH, new Color(180, 160, 140), labelFontScale);
        DrawText(spriteBatch, "EXP", c2X - 5, sY + rowH * 2, new Color(180, 160, 140), labelFontScale);
        DrawText(spriteBatch, "GOLD", c2X - 15, sY + rowH * 3, new Color(180, 160, 140), labelFontScale);

        int b2X = c2X + col2LblW;
        int b2W = 120;
        int b2H = 16;
        UIHelper.DrawInsetBox(spriteBatch, new Rectangle(b2X, sY + 1, b2W, b2H)); // HP
        UIHelper.DrawInsetBox(spriteBatch, new Rectangle(b2X, sY + rowH + 1, b2W, b2H)); // MP
        UIHelper.DrawInsetBox(spriteBatch, new Rectangle(b2X, sY + rowH * 2 + 1, b2W, b2H)); // EXP
        UIHelper.DrawInsetBox(spriteBatch, new Rectangle(b2X, sY + rowH * 3 + 1, b2W, b2H)); // GOLD

        DrawTextRightAligned(spriteBatch, player.CurrentHP.ToString(), b2X + 55, sY, Color.Wheat, valueFontScale);
        DrawTextRightAligned(spriteBatch, player.MaxHP.ToString(), b2X + b2W - 5, sY, new Color(150, 130, 110), valueFontScale);
        
        DrawTextRightAligned(spriteBatch, player.CurrentMP.ToString(), b2X + 55, sY + rowH, Color.Wheat, valueFontScale);
        DrawTextRightAligned(spriteBatch, player.MaxMP.ToString(), b2X + b2W - 5, sY + rowH, new Color(150, 130, 110), valueFontScale);

        DrawTextRightAligned(spriteBatch, player.Experience.ToString(), b2X + b2W - 5, sY + rowH * 2, Color.Wheat, valueFontScale);
        DrawTextRightAligned(spriteBatch, "1000000", b2X + b2W - 5, sY + rowH * 3, Color.Wheat, valueFontScale);

        // Col 3
        int c3X = c2X + 180;
        DrawText(spriteBatch, "Level", c3X, sY, new Color(180, 160, 140), labelFontScale);
        DrawText(spriteBatch, "next LEV", c3X - 25, sY + rowH, new Color(180, 160, 140), labelFontScale);
        
        int b3X = c3X + 45;
        int b3W = 40;
        UIHelper.DrawInsetBox(spriteBatch, new Rectangle(b3X, sY + 1, b3W, b2H));
        UIHelper.DrawInsetBox(spriteBatch, new Rectangle(b3X, sY + rowH + 1, b3W, b2H));
        
        DrawTextRightAligned(spriteBatch, player.Level.ToString(), b3X + b3W - 5, sY, Color.Wheat, valueFontScale);
        DrawTextRightAligned(spriteBatch, "0", b3X + b3W - 5, sY + rowH, Color.Wheat, valueFontScale);

        // Bottom Strip
        int stripY = sY + rowH * 4 + 8;
        string coordStr = $"X Y   {(int)player.WorldPosition.X}, {(int)player.WorldPosition.Y}";
        DrawText(spriteBatch, coordStr, statStartX + 20, stripY, new Color(200, 180, 150), labelFontScale * 0.9f);
        DrawText(spriteBatch, "zone", statStartX + 120, stripY - 3, new Color(150, 130, 100), labelFontScale * 0.7f);
        DrawText(spriteBatch, "Inn", statStartX + 110, stripY + 7, Color.White, labelFontScale * 0.9f);
        DrawText(spriteBatch, "weight", statStartX + 200, stripY - 3, new Color(150, 130, 100), labelFontScale * 0.7f);
        DrawText(spriteBatch, "50 / 100", statStartX + 190, stripY + 7, Color.White, labelFontScale * 0.9f);
        DrawText(spriteBatch, "Native", statStartX + 300, stripY, new Color(150, 130, 100), labelFontScale * 0.9f);

        // ==========================================
        // RIGHT PANEL (520 to 800) Width: 280
        // ==========================================
        DrawText(spriteBatch, "ID", rX + 40, hudY + 10, new Color(150, 130, 100), labelFontScale);
        UIHelper.DrawInsetBox(spriteBatch, new Rectangle(rX + 10, hudY + 25, 120, 20));
        DrawText(spriteBatch, player.Name, rX + 25, hudY + 25, Color.White, valueFontScale);

        // Buttons
        for (int i=0; i<3; i++)
        {
            for(int j=0; j<2; j++)
            {
                spriteBatch.Draw(_whitePixel, new Rectangle(rX + 35 + j*35, hudY + 55 + i*25, 20, 20), new Color(100, 80, 60));
            }
        }

        DrawText(spriteBatch, "SHOP", rX + 45, hudY + 130, new Color(200, 180, 100), labelFontScale);
        
        // ==========================================
        // FLOATING PARTY OVERLAY
        // ==========================================
        int compIndex = 0;
        foreach (var comp in companions)
        {
            if (!comp.IsRecruited) continue;
            int cY = hudY - 30 - (compIndex * 30);
            int cXp = 10; 
            
            spriteBatch.Draw(_whitePixel, new Rectangle(cXp, cY, 180, 25), new Color(0, 0, 0, 150));
            DrawText(spriteBatch, $"[파티] {comp.Name}", cXp + 5, cY + 2, Color.LightGreen, labelFontScale);
            
            Rectangle cRect = new Rectangle(cXp + 100, cY + 10, 70, 8);
            spriteBatch.Draw(_whitePixel, cRect, new Color(40, 0, 0));
            float cRatio = comp.MaxHP > 0 ? (float)comp.CurrentHP / comp.MaxHP : 0f;
            int fillW = (int)(cRect.Width * cRatio);
            spriteBatch.Draw(_whitePixel, new Rectangle(cRect.X, cRect.Y, fillW, cRect.Height), Color.Crimson);
            
            compIndex++;
        }
    }

    private void DrawText(SpriteBatch sb, string text, int x, int y, Color color, float scale)
    {
        FontManager.DrawString(sb, text, new Vector2(x, y), color, scale);
    }

    private void DrawTextRightAligned(SpriteBatch sb, string text, int rightX, int y, Color color, float scale)
    {
        float width = FontManager.MeasureString(text, scale).X;
        FontManager.DrawString(sb, text, new Vector2(rightX - width, y), color, scale);
    }
}
