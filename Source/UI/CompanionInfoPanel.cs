using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using KingdomOfDarkness.Core;
using KingdomOfDarkness.Entities;

namespace KingdomOfDarkness.UI;

public class CompanionInfoPanel
{
    private readonly Texture2D _whitePixel;
    public Companion TargetCompanion { get; private set; }
    
    private Rectangle _panelRect;
    private Rectangle _buttonRect;
    private bool _isVisible = false;

    public CompanionInfoPanel(Texture2D whitePixel)
    {
        _whitePixel = whitePixel;
    }

    public void Open(Companion companion)
    {
        TargetCompanion = companion;
        _isVisible = true;

        // Center panel on screen
        int panelW = 300;
        int panelH = 200;
        _panelRect = new Rectangle(
            GameConstants.ScreenWidth / 2 - panelW / 2,
            GameConstants.ScreenHeight / 2 - panelH / 2,
            panelW,
            panelH
        );

        // Button positioned at bottom of panel
        _buttonRect = new Rectangle(
            _panelRect.X + 50,
            _panelRect.Y + 140,
            200,
            40
        );
    }

    public void Close()
    {
        _isVisible = false;
        TargetCompanion = null;
    }

    public bool Update(InputManager inputManager)
    {
        if (!_isVisible || TargetCompanion == null) return false;

        // If user left clicks...
        if (inputManager.IsLeftMouseClicked())
        {
            Vector2 mousePos = inputManager.MousePosition;

            // Check if clicked the button
            if (_buttonRect.Contains(mousePos.X, mousePos.Y))
            {
                // Accept party!
                TargetCompanion.IsRecruited = true;
                TargetCompanion.State = CompanionState.FollowPlayer; // Start following
                Close();
                return true; // Returns true if an action was handled
            }

            // Check if clicked outside panel to close it
            if (!_panelRect.Contains(mousePos.X, mousePos.Y))
            {
                Close();
            }
        }

        return false;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        if (!_isVisible || TargetCompanion == null) return;

        // Background
        spriteBatch.Draw(_whitePixel, _panelRect, new Color(15, 10, 20, 240));

        // Outline
        DrawOutline(spriteBatch, _panelRect, Color.DarkOrchid, 2);

        // Content
        Vector2 textPos = new Vector2(_panelRect.X + 20, _panelRect.Y + 20);
        
        FontManager.DrawString(spriteBatch, $"[동료 정보]", textPos, Color.Gold, 0.8f);
        textPos.Y += 30;
        
        FontManager.DrawString(spriteBatch, $"이름: {TargetCompanion.Name}", textPos, Color.White, 0.7f);
        textPos.Y += 25;
        
        FontManager.DrawString(spriteBatch, $"직업: {TargetCompanion.ClassType}", textPos, Color.LightGray, 0.7f);
        textPos.Y += 25;
        
        FontManager.DrawString(spriteBatch, $"레벨: {TargetCompanion.Level}", textPos, Color.LightGray, 0.7f);

        // Draw Button
        spriteBatch.Draw(_whitePixel, _buttonRect, new Color(40, 100, 50));
        DrawOutline(spriteBatch, _buttonRect, Color.LimeGreen, 1);
        
        string btnText = "파티 영입 신청 (수락)";
        Vector2 btnTextSize = FontManager.MeasureString(btnText, 0.7f);
        Vector2 btnTextPos = new Vector2(
            _buttonRect.X + _buttonRect.Width / 2 - btnTextSize.X / 2,
            _buttonRect.Y + _buttonRect.Height / 2 - btnTextSize.Y / 2
        );
        FontManager.DrawString(spriteBatch, btnText, btnTextPos, Color.White, 0.7f);
    }

    private void DrawOutline(SpriteBatch sb, Rectangle rect, Color color, int thickness)
    {
        sb.Draw(_whitePixel, new Rectangle(rect.X, rect.Y, rect.Width, thickness), color);
        sb.Draw(_whitePixel, new Rectangle(rect.X, rect.Y + rect.Height - thickness, rect.Width, thickness), color);
        sb.Draw(_whitePixel, new Rectangle(rect.X, rect.Y, thickness, rect.Height), color);
        sb.Draw(_whitePixel, new Rectangle(rect.X + rect.Width - thickness, rect.Y, thickness, rect.Height), color);
    }
}
