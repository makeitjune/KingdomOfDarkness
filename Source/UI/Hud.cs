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

    public Hud(Texture2D whitePixel)
    {
        _whitePixel = whitePixel;
    }

    public void Draw(SpriteBatch spriteBatch, Player player, List<Companion> companions, Texture2D whitePixel)
    {
        int logicalWidth = (int)GameConstants.LogicalScreenWidth;
        int logicalHeight = (int)GameConstants.LogicalScreenHeight;
        
        // 1. Dynamic Height (38% of screen height)
        int panelHeight = (int)(logicalHeight * 0.38f);
        int hudY = logicalHeight - panelHeight;
        
        // Dynamic scale relative to 160px baseline
        float scale = panelHeight / 160f;

        // Draw Full Width Background
        Rectangle bottomRect = new Rectangle(0, hudY, logicalWidth, panelHeight);
        UIHelper.DrawRetroPanel(spriteBatch, bottomRect);

        // Calculate absolute scaled dimensions
        int leftWidth = (int)(130 * scale);
        int rightWidth = (int)(150 * scale);
        int centerWidth = logicalWidth - leftWidth - rightWidth;
        int cX = leftWidth;
        int rX = logicalWidth - rightWidth;

        // Base font scales
        float labelFontScale = 0.85f * scale;
        float valueFontScale = 0.95f * scale;

        // ==========================================
        // LEFT PANEL (Anchored Left)
        // ==========================================
        int orbW = (int)(30 * scale);
        int orbH = (int)(100 * scale);
        int orbY = hudY + (int)(30 * scale);

        // HP Text
        UIHelper.DrawInsetBox(spriteBatch, new Rectangle(10, hudY + (int)(10 * scale), (int)(100 * scale), (int)(16 * scale)));
        DrawTextRightAligned(spriteBatch, player.CurrentHP.ToString(), 10 + (int)(95 * scale), hudY + (int)(10 * scale), new Color(255, 100, 100), valueFontScale);
        
        // HP Orb
        Rectangle hpRect = new Rectangle(15, orbY, orbW, orbH);
        float hpRatio = player.MaxHP > 0 ? (float)player.CurrentHP / player.MaxHP : 0f;
        UIHelper.DrawHpMpOrb(spriteBatch, hpRect, hpRatio, new Color(220, 30, 30));

        // MP Orb
        Rectangle mpRect = new Rectangle(15 + orbW + (int)(15 * scale), orbY, orbW, orbH);
        float mpRatio = player.MaxMP > 0 ? (float)player.CurrentMP / player.MaxMP : 0f;
        UIHelper.DrawHpMpOrb(spriteBatch, mpRect, mpRatio, new Color(30, 100, 240));

        // MP Text
        int mpTextY = orbY + orbH + (int)(5 * scale);
        UIHelper.DrawInsetBox(spriteBatch, new Rectangle(10, mpTextY, (int)(100 * scale), (int)(16 * scale)));
        DrawTextRightAligned(spriteBatch, player.CurrentMP.ToString(), 10 + (int)(95 * scale), mpTextY, new Color(100, 150, 255), valueFontScale);


        // ==========================================
        // CENTER PANEL (Stretched)
        // ==========================================
        
        // Top Message Box (Full width of center panel)
        int msgY = hudY + (int)(10 * scale);
        int msgH = (int)(22 * scale);
        UIHelper.DrawInsetBox(spriteBatch, new Rectangle(cX, msgY, centerWidth - (int)(10 * scale), msgH));
        DrawText(spriteBatch, "환영합니다.", cX + (int)(10 * scale), msgY + (int)(3 * scale), new Color(220, 180, 120), labelFontScale);

        // Stat Area (Centered within Center Panel)
        int statTotalWidth = (int)(450 * scale);
        int statStartX = cX + (centerWidth - statTotalWidth) / 2;
        int sY = msgY + msgH + (int)(10 * scale);
        int rowH = (int)(20 * scale); // Taller rows!

        // Col 1
        string[] stats = { "STR", "INT", "WIS", "CON", "DEX" };
        int[] statVals = { 5, 97, 109, 4, 3 }; 
        for (int i = 0; i < 5; i++)
        {
            int y = sY + (i * rowH);
            DrawText(spriteBatch, stats[i], statStartX, y, new Color(180, 160, 140), labelFontScale);
            UIHelper.DrawInsetBox(spriteBatch, new Rectangle(statStartX + (int)(40 * scale), y + 1, (int)(30 * scale), (int)(16 * scale)));
            DrawTextRightAligned(spriteBatch, statVals[i].ToString(), statStartX + (int)(65 * scale), y, Color.Wheat, valueFontScale);
            spriteBatch.Draw(_whitePixel, new Rectangle(statStartX + (int)(75 * scale), y + 3, (int)(10 * scale), (int)(10 * scale)), new Color(100, 80, 60));
        }

        // Col 2
        int c2X = statStartX + (int)(120 * scale);
        int col2LblW = (int)(50 * scale);
        DrawText(spriteBatch, "HP", c2X, sY, new Color(180, 160, 140), labelFontScale);
        DrawText(spriteBatch, "MP", c2X, sY + rowH, new Color(180, 160, 140), labelFontScale);
        DrawText(spriteBatch, "EXP", c2X - (int)(5 * scale), sY + rowH * 2, new Color(180, 160, 140), labelFontScale);
        DrawText(spriteBatch, "GOLD", c2X - (int)(15 * scale), sY + rowH * 3, new Color(180, 160, 140), labelFontScale);

        int b2X = c2X + col2LblW;
        int b2W = (int)(140 * scale);
        int b2H = (int)(16 * scale);
        UIHelper.DrawInsetBox(spriteBatch, new Rectangle(b2X, sY + 1, b2W, b2H)); // HP
        UIHelper.DrawInsetBox(spriteBatch, new Rectangle(b2X, sY + rowH + 1, b2W, b2H)); // MP
        UIHelper.DrawInsetBox(spriteBatch, new Rectangle(b2X, sY + rowH * 2 + 1, b2W, b2H)); // EXP
        UIHelper.DrawInsetBox(spriteBatch, new Rectangle(b2X, sY + rowH * 3 + 1, b2W, b2H)); // GOLD

        DrawTextRightAligned(spriteBatch, player.CurrentHP.ToString(), b2X + (int)(65 * scale), sY, Color.Wheat, valueFontScale);
        DrawTextRightAligned(spriteBatch, player.MaxHP.ToString(), b2X + b2W - (int)(5 * scale), sY, new Color(150, 130, 110), valueFontScale);
        
        DrawTextRightAligned(spriteBatch, player.CurrentMP.ToString(), b2X + (int)(65 * scale), sY + rowH, Color.Wheat, valueFontScale);
        DrawTextRightAligned(spriteBatch, player.MaxMP.ToString(), b2X + b2W - (int)(5 * scale), sY + rowH, new Color(150, 130, 110), valueFontScale);

        DrawTextRightAligned(spriteBatch, player.Experience.ToString(), b2X + b2W - (int)(5 * scale), sY + rowH * 2, Color.Wheat, valueFontScale);
        DrawTextRightAligned(spriteBatch, "1000000", b2X + b2W - (int)(5 * scale), sY + rowH * 3, Color.Wheat, valueFontScale);

        // Col 3
        int c3X = c2X + (int)(210 * scale);
        DrawText(spriteBatch, "Level", c3X, sY, new Color(180, 160, 140), labelFontScale);
        DrawText(spriteBatch, "next LEV", c3X - (int)(25 * scale), sY + rowH, new Color(180, 160, 140), labelFontScale);
        
        int b3X = c3X + (int)(50 * scale);
        int b3W = (int)(45 * scale);
        UIHelper.DrawInsetBox(spriteBatch, new Rectangle(b3X, sY + 1, b3W, b2H));
        UIHelper.DrawInsetBox(spriteBatch, new Rectangle(b3X, sY + rowH + 1, b3W, b2H));
        
        DrawTextRightAligned(spriteBatch, player.Level.ToString(), b3X + b3W - (int)(5 * scale), sY, Color.Wheat, valueFontScale);
        DrawTextRightAligned(spriteBatch, "0", b3X + b3W - (int)(5 * scale), sY + rowH, Color.Wheat, valueFontScale);

        // Bottom Strip (Coordinates, Zone)
        int stripY = sY + rowH * 4 + (int)(10 * scale);
        string coordStr = $"X Y   {(int)player.WorldPosition.X}, {(int)player.WorldPosition.Y}";
        DrawText(spriteBatch, coordStr, statStartX + (int)(20 * scale), stripY, new Color(200, 180, 150), labelFontScale * 0.9f);
        DrawText(spriteBatch, "zone", statStartX + (int)(150 * scale), stripY - (int)(3 * scale), new Color(150, 130, 100), labelFontScale * 0.7f);
        DrawText(spriteBatch, "여관", statStartX + (int)(140 * scale), stripY + (int)(5 * scale), Color.White, labelFontScale * 0.9f);
        DrawText(spriteBatch, "weight", statStartX + (int)(250 * scale), stripY - (int)(3 * scale), new Color(150, 130, 100), labelFontScale * 0.7f);
        DrawText(spriteBatch, "50 / 100", statStartX + (int)(240 * scale), stripY + (int)(5 * scale), Color.White, labelFontScale * 0.9f);
        DrawText(spriteBatch, "Native", statStartX + (int)(350 * scale), stripY, new Color(150, 130, 100), labelFontScale * 0.9f);


        // ==========================================
        // RIGHT PANEL (Anchored Right)
        // ==========================================
        DrawText(spriteBatch, "ID", rX + (int)(40 * scale), hudY + (int)(10 * scale), new Color(150, 130, 100), labelFontScale);
        UIHelper.DrawInsetBox(spriteBatch, new Rectangle(rX + (int)(10 * scale), hudY + (int)(25 * scale), (int)(120 * scale), (int)(20 * scale)));
        DrawText(spriteBatch, player.Name, rX + (int)(25 * scale), hudY + (int)(25 * scale), Color.White, valueFontScale);

        // Buttons
        for (int i=0; i<3; i++)
        {
            for(int j=0; j<2; j++)
            {
                spriteBatch.Draw(_whitePixel, new Rectangle(rX + (int)(35 * scale) + j*(int)(35 * scale), hudY + (int)(55 * scale) + i*(int)(25 * scale), (int)(20 * scale), (int)(20 * scale)), new Color(100, 80, 60));
            }
        }

        DrawText(spriteBatch, "SHOP", rX + (int)(45 * scale), hudY + (int)(135 * scale), new Color(200, 180, 100), labelFontScale);
        
        // ==========================================
        // FLOATING PARTY OVERLAY
        // ==========================================
        int compIndex = 0;
        foreach (var comp in companions)
        {
            if (!comp.IsRecruited) continue;
            int cY = hudY - (int)(30 * scale) - (compIndex * (int)(30 * scale));
            int cXp = 10; 
            
            spriteBatch.Draw(_whitePixel, new Rectangle(cXp, cY, (int)(180 * scale), (int)(25 * scale)), new Color(0, 0, 0, 150));
            DrawText(spriteBatch, $"[파티] {comp.Name}", cXp + 5, cY + 2, Color.LightGreen, labelFontScale);
            
            Rectangle cRect = new Rectangle(cXp + (int)(100 * scale), cY + (int)(10 * scale), (int)(70 * scale), (int)(8 * scale));
            spriteBatch.Draw(_whitePixel, cRect, new Color(40, 0, 0));
            float cRatio = comp.MaxHP > 0 ? (float)comp.CurrentHP / comp.MaxHP : 0f;
            int fillW = (int)(cRect.Width * cRatio);
            spriteBatch.Draw(_whitePixel, new Rectangle(cRect.X, cRect.Y, fillW, cRect.Height), Color.Crimson);
            
            compIndex++;
        }

        // ==========================================
        // FLOATING TARGET OVERLAY (Removed per request)
        // ==========================================
        /*
        if (player.Target != null && !player.Target.IsDead)
        {
            int tX = logicalWidth / 2 - (int)(120 * scale);
            int tY = 10;
            UIHelper.DrawInsetBox(spriteBatch, new Rectangle(tX, tY, (int)(240 * scale), (int)(40 * scale)));
            DrawText(spriteBatch, player.Target.Name, tX + (int)(10 * scale), tY + 5, Color.OrangeRed, valueFontScale);
            
            Rectangle tHpRect = new Rectangle(tX + (int)(10 * scale), tY + (int)(25 * scale), (int)(220 * scale), (int)(10 * scale));
            spriteBatch.Draw(_whitePixel, tHpRect, new Color(40, 0, 0));
            float tRatio = (float)player.Target.CurrentHP / player.Target.MaxHP;
            spriteBatch.Draw(_whitePixel, new Rectangle(tHpRect.X, tHpRect.Y, (int)(tHpRect.Width * tRatio), tHpRect.Height), Color.Crimson);
        }
        */
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
