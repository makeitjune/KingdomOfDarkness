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
        // Draw bottom party status panel
        DrawPartyPanel(spriteBatch, player, companions);

        // Draw top target monster panel if player has a live target
        if (player.Target != null && !player.Target.IsDead)
        {
            DrawTargetPanel(spriteBatch, player.Target);
        }
    }

    private void DrawPartyPanel(SpriteBatch spriteBatch, Player player, List<Companion> companions)
    {
        int recruitedCount = 0;
        foreach (var c in companions) { if (c.IsRecruited) recruitedCount++; }

        int panelW = 380;
        int panelH = 70 + (recruitedCount * 40); // Dynamic height
        Vector2 panelPos = new Vector2(20, GameConstants.LogicalScreenHeight - panelH - 20);

        // 1. Draw Panel Background (Glassmorphism dark gray)
        spriteBatch.Draw(
            _whitePixel,
            new Rectangle((int)panelPos.X, (int)panelPos.Y, panelW, panelH),
            new Color(10, 14, 18, 220)
        );

        // Border
        DrawOutline(spriteBatch, new Rectangle((int)panelPos.X, (int)panelPos.Y, panelW, panelH), new Color(100, 110, 120, 150), 1);

        // Header
        FontManager.DrawString(spriteBatch, "파티 상태", panelPos + new Vector2(15, 12), Color.Gold, 0.85f);

        // 2. Draw Player Stats
        Vector2 playerStatsPos = panelPos + new Vector2(15, 36);
        string playerText = $"[{player.ClassType}] {player.Name}  Lv.{player.Level}";
        FontManager.DrawString(spriteBatch, playerText, playerStatsPos, Color.White, 0.7f);

        // HP/MP Bar
        Vector2 playerHpPos = playerStatsPos + new Vector2(0, 14);
        DrawStatusBar(spriteBatch, playerHpPos, 100, 6, player.CurrentHP, player.MaxHP, Color.Crimson, "HP");
        DrawStatusBar(spriteBatch, playerHpPos + new Vector2(140, 0), 80, 6, player.CurrentMP, player.MaxMP, Color.DodgerBlue, "MP");
        string playerHpStr = $"{player.CurrentHP}/{player.MaxHP}";
        FontManager.DrawString(spriteBatch, playerHpStr, playerHpPos + new Vector2(250, 0), Color.LightGray, 0.60f);

        // 3. Draw Companion Stats
        int compIndex = 0;
        foreach (var comp in companions)
        {
            if (!comp.IsRecruited) continue;
            Vector2 compStatsPos = playerStatsPos + new Vector2(0, 36 + (compIndex * 40));
            string compText = $"[{comp.ClassType}] {comp.Name}  Lv.{comp.Level}";
            FontManager.DrawString(spriteBatch, compText, compStatsPos, Color.White, 0.65f);

            // HP/MP Bar
            Vector2 compHpPos = compStatsPos + new Vector2(0, 14);
            DrawStatusBar(spriteBatch, compHpPos, 100, 6, comp.CurrentHP, comp.MaxHP, Color.Crimson, "HP");
            DrawStatusBar(spriteBatch, compHpPos + new Vector2(140, 0), 80, 6, comp.CurrentMP, comp.MaxMP, Color.DodgerBlue, "MP");
            string compHpStr = $"{comp.CurrentHP}/{comp.MaxHP}";
            FontManager.DrawString(spriteBatch, compHpStr, compHpPos + new Vector2(250, 0), Color.LightGray, 0.60f);

            // Companion State text
            string stateStr = GetKoreanState(comp.State);
            FontManager.DrawString(spriteBatch, stateStr, compStatsPos + new Vector2(250, -4), Color.Plum, 0.6f);
            
            compIndex++;
        }
    }

    private void DrawTargetPanel(SpriteBatch spriteBatch, Character target)
    {
        int panelW = 280;
        int panelH = 65;
        // Top right corner using logical width
        Vector2 panelPos = new Vector2(GameConstants.LogicalScreenWidth - panelW - 20, 20);

        // Draw Panel Background (Glassmorphism dark gray with subtle red glow border for hostility)
        spriteBatch.Draw(
            _whitePixel,
            new Rectangle((int)panelPos.X, (int)panelPos.Y, panelW, panelH),
            new Color(15, 10, 10, 230)
        );

        DrawOutline(spriteBatch, new Rectangle((int)panelPos.X, (int)panelPos.Y, panelW, panelH), new Color(180, 50, 50, 180), 1);

        // Target Info text
        Vector2 namePos = panelPos + new Vector2(15, 12);
        string nameStr = $"{target.Name}  Lv.{target.Level}";
        FontManager.DrawString(spriteBatch, nameStr, namePos, Color.OrangeRed, 0.75f);

        // HP Bar
        Vector2 hpPos = namePos + new Vector2(0, 18);
        DrawStatusBar(spriteBatch, hpPos, 160, 10, target.CurrentHP, target.MaxHP, Color.Crimson, "HP");
        string hpStr = $"{target.CurrentHP}/{target.MaxHP}";
        FontManager.DrawString(spriteBatch, hpStr, hpPos + new Vector2(170, 0), Color.LightGray, 0.65f);
    }

    private void DrawStatusBar(SpriteBatch sb, Vector2 pos, int width, int height, int current, int max, Color color, string prefix)
    {
        // prefix text if any
        float textOffset = 0f;
        if (!string.IsNullOrEmpty(prefix))
        {
            FontManager.DrawString(sb, prefix, pos, color, 0.65f);
            textOffset = FontManager.MeasureString(prefix, 0.65f).X + 4f;
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

    private string GetKoreanState(CompanionState state)
    {
        return state switch
        {
            CompanionState.Idle => "대기",
            CompanionState.FollowPlayer => "따라가기",
            CompanionState.AssistAttack => "전투중",
            CompanionState.Retreat => "후퇴",
            CompanionState.Dead => "사망",
            _ => "알 수 없음",
        };
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
