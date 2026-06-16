using System;
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

    public void Draw(SpriteBatch spriteBatch, Player player, Companion companion, Texture2D whitePixel)
    {
        // Draw bottom party status panel
        DrawPartyPanel(spriteBatch, player, companion);

        // Draw top target monster panel if player has a live target
        if (player.Target != null && !player.Target.IsDead)
        {
            DrawTargetPanel(spriteBatch, player.Target);
        }
    }

    private void DrawPartyPanel(SpriteBatch spriteBatch, Player player, Companion companion)
    {
        int panelW = 380;
        int panelH = 140;
        Vector2 panelPos = new Vector2(20, GameConstants.ScreenHeight - panelH - 20);

        // 1. Draw Panel Background (Glassmorphism dark gray)
        spriteBatch.Draw(
            _whitePixel,
            new Rectangle((int)panelPos.X, (int)panelPos.Y, panelW, panelH),
            new Color(10, 14, 18, 220)
        );

        // Border
        DrawOutline(spriteBatch, new Rectangle((int)panelPos.X, (int)panelPos.Y, panelW, panelH), new Color(100, 110, 120, 150), 1);

        // Header
        SimpleFont.DrawString(spriteBatch, _whitePixel, "PARTY STATUS", panelPos + new Vector2(15, 12), Color.Gold, 1.0f);

        // 2. Draw Player Stats
        Vector2 playerStatsPos = panelPos + new Vector2(15, 36);
        string playerText = $"PLAYER  LVL {player.Level}";
        SimpleFont.DrawString(spriteBatch, _whitePixel, playerText, playerStatsPos, Color.White, 0.8f);

        // HP Bar
        Vector2 playerHpPos = playerStatsPos + new Vector2(0, 14);
        DrawStatusBar(spriteBatch, playerHpPos, 160, 10, player.CurrentHP, player.MaxHP, Color.Crimson, "HP");
        string playerHpStr = $"{player.CurrentHP}/{player.MaxHP}";
        SimpleFont.DrawString(spriteBatch, _whitePixel, playerHpStr, playerHpPos + new Vector2(170, 0), Color.LightGray, 0.7f);

        // EXP Bar (Standard LevelSystem 100 XP per level)
        Vector2 playerExpPos = playerHpPos + new Vector2(0, 14);
        int expNeeded = player.Level * 100;
        DrawStatusBar(spriteBatch, playerExpPos, 160, 6, player.Experience, expNeeded, Color.DodgerBlue, "XP");
        string playerExpStr = $"{player.Experience}/{expNeeded} EXP";
        SimpleFont.DrawString(spriteBatch, _whitePixel, playerExpStr, playerExpPos + new Vector2(170, 0), Color.LightGray, 0.7f);

        // 3. Draw Companion Stats
        Vector2 compStatsPos = panelPos + new Vector2(15, 86);
        string compText = $"COMPANION  LVL {companion.Level}";
        SimpleFont.DrawString(spriteBatch, _whitePixel, compText, compStatsPos, Color.White, 0.8f);

        // HP Bar
        Vector2 compHpPos = compStatsPos + new Vector2(0, 14);
        DrawStatusBar(spriteBatch, compHpPos, 160, 10, companion.CurrentHP, companion.MaxHP, Color.Crimson, "HP");
        string compHpStr = $"{companion.CurrentHP}/{companion.MaxHP}";
        SimpleFont.DrawString(spriteBatch, _whitePixel, compHpStr, compHpPos + new Vector2(170, 0), Color.LightGray, 0.7f);

        // Companion State text
        string stateStr = $"STATE: {companion.State.ToString().ToUpper()}";
        SimpleFont.DrawString(spriteBatch, _whitePixel, stateStr, compHpPos + new Vector2(170, -12), Color.Plum, 0.7f);
    }

    private void DrawTargetPanel(SpriteBatch spriteBatch, Character target)
    {
        int panelW = 280;
        int panelH = 70;
        Vector2 panelPos = new Vector2(GameConstants.ScreenWidth / 2f - panelW / 2f, 20);

        // Draw Panel Background (Glassmorphism dark gray with subtle red glow border for hostility)
        spriteBatch.Draw(
            _whitePixel,
            new Rectangle((int)panelPos.X, (int)panelPos.Y, panelW, panelH),
            new Color(15, 10, 10, 230)
        );

        DrawOutline(spriteBatch, new Rectangle((int)panelPos.X, (int)panelPos.Y, panelW, panelH), new Color(180, 50, 50, 180), 1);

        // Target Info text
        Vector2 namePos = panelPos + new Vector2(15, 12);
        string nameStr = $"{target.Name.ToUpper()}  LVL {target.Level}";
        SimpleFont.DrawString(spriteBatch, _whitePixel, nameStr, namePos, Color.OrangeRed, 0.9f);

        // HP Bar
        Vector2 hpPos = namePos + new Vector2(0, 18);
        DrawStatusBar(spriteBatch, hpPos, 160, 10, target.CurrentHP, target.MaxHP, Color.Crimson, "HP");
        string hpStr = $"{target.CurrentHP}/{target.MaxHP}";
        SimpleFont.DrawString(spriteBatch, _whitePixel, hpStr, hpPos + new Vector2(170, 0), Color.LightGray, 0.7f);
    }

    private void DrawStatusBar(SpriteBatch sb, Vector2 pos, int width, int height, int current, int max, Color color, string prefix)
    {
        // prefix text if any
        float textOffset = 0f;
        if (!string.IsNullOrEmpty(prefix))
        {
            SimpleFont.DrawString(sb, _whitePixel, prefix, pos, color, 0.7f);
            textOffset = 18f;
        }

        Vector2 barPos = pos + new Vector2(textOffset, 0f);

        // Background bar
        sb.Draw(
            _whitePixel,
            new Rectangle((int)barPos.X, (int)barPos.Y, width, height),
            new Color(40, 40, 45)
        );

        // Filled bar
        float ratio = max > 0 ? (float)current / max : 0f;
        int fillW = (int)(width * MathHelper.Clamp(ratio, 0f, 1f));

        sb.Draw(
            _whitePixel,
            new Rectangle((int)barPos.X, (int)barPos.Y, fillW, height),
            color
        );
    }

    private void DrawOutline(SpriteBatch sb, Rectangle rect, Color color, int thickness)
    {
        // Top
        sb.Draw(_whitePixel, new Rectangle(rect.X, rect.Y, rect.Width, thickness), color);
        // Bottom
        sb.Draw(_whitePixel, new Rectangle(rect.X, rect.Y + rect.Height - thickness, rect.Width, thickness), color);
        // Left
        sb.Draw(_whitePixel, new Rectangle(rect.X, rect.Y, thickness, rect.Height), color);
        // Right
        sb.Draw(_whitePixel, new Rectangle(rect.X + rect.Width - thickness, rect.Y, thickness, rect.Height), color);
    }
}
