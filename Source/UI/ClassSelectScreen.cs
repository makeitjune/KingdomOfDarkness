using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using KingdomOfDarkness.Core;
using KingdomOfDarkness.Data;

namespace KingdomOfDarkness.UI;

public class ClassSelectScreen
{
    private Texture2D _whitePixel;

    public ClassSelectScreen(Texture2D whitePixel)
    {
        _whitePixel = whitePixel;
    }

    public CharacterClassType? Update(InputManager inputManager)
    {
        // Keyboard checks
        if (inputManager.IsKeyPressed(Keys.D1) || inputManager.IsKeyPressed(Keys.NumPad1)) return CharacterClassType.Warrior;
        if (inputManager.IsKeyPressed(Keys.D2) || inputManager.IsKeyPressed(Keys.NumPad2)) return CharacterClassType.Mage;
        if (inputManager.IsKeyPressed(Keys.D3) || inputManager.IsKeyPressed(Keys.NumPad3)) return CharacterClassType.Priest;
        if (inputManager.IsKeyPressed(Keys.D4) || inputManager.IsKeyPressed(Keys.NumPad4)) return CharacterClassType.Rogue;
        if (inputManager.IsKeyPressed(Keys.D5) || inputManager.IsKeyPressed(Keys.NumPad5)) return CharacterClassType.Monk;

        // Mouse checks
        if (inputManager.IsLeftMouseClicked())
        {
            Vector2 mousePos = inputManager.MousePosition;
            float virtualW = GameConstants.VirtualWidth;
            
            int startY = 150;
            int spacing = 50;

            for (int i = 0; i < 5; i++)
            {
                Rectangle bounds = new Rectangle((int)(virtualW / 2f - 200), startY + (spacing * i), 400, 30);
                if (bounds.Contains(mousePos))
                {
                    return (CharacterClassType)i;
                }
            }
        }

        return null;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        float virtualW = GameConstants.VirtualWidth;
        float virtualH = GameConstants.VirtualHeight;

        // Background
        spriteBatch.Draw(_whitePixel, new Rectangle(0, 0, (int)virtualW, (int)virtualH), new Color(15, 15, 20));

        // Title
        string title = "어둠의 전설 - 직업 선택";
        Vector2 titleSize = FontManager.MeasureString(title, 1.0f);
        FontManager.DrawString(spriteBatch, title, new Vector2(virtualW / 2f - titleSize.X / 2f, 50), Color.Gold, 1.0f);

        // Options
        int startY = 150;
        int spacing = 50;

        DrawOption(spriteBatch, "1. 전사 (Warrior) - 체력/방어", new Vector2(virtualW / 2f - 200, startY), Color.CornflowerBlue);
        DrawOption(spriteBatch, "2. 마법사 (Mage) - 광역 마법", new Vector2(virtualW / 2f - 200, startY + spacing), Color.MediumPurple);
        DrawOption(spriteBatch, "3. 성직자 (Priest) - 아군 회복/버프", new Vector2(virtualW / 2f - 200, startY + spacing * 2), Color.LightGoldenrodYellow);
        DrawOption(spriteBatch, "4. 도적 (Rogue) - 속도/치명타", new Vector2(virtualW / 2f - 200, startY + spacing * 3), Color.DarkSlateGray);
        DrawOption(spriteBatch, "5. 무도가 (Monk) - 근접 난타", new Vector2(virtualW / 2f - 200, startY + spacing * 4), Color.SaddleBrown);
        
        string prompt = "원하는 직업을 마우스로 클릭하거나 숫자키를 누르세요.";
        Vector2 promptSize = FontManager.MeasureString(prompt, 0.7f);
        FontManager.DrawString(spriteBatch, prompt, new Vector2(virtualW / 2f - promptSize.X / 2f, virtualH - 50), Color.LightGray, 0.7f);
    }

    private void DrawOption(SpriteBatch spriteBatch, string text, Vector2 position, Color color)
    {
        FontManager.DrawString(spriteBatch, text, position, color, 0.9f);
    }
}
