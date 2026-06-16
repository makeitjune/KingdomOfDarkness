using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using KingdomOfDarkness.Core;

namespace KingdomOfDarkness.UI;

/// <summary>
/// Manages all active damage popup numbers. Handles spawning, updating, and drawing.
/// </summary>
public class DamagePopupManager
{
    private readonly List<DamagePopup> _popups = new();

    /// <summary>
    /// Spawns a new damage popup at the target's world position.
    /// </summary>
    public void Spawn(Vector2 worldPosition, int damage, bool isCritical = false, bool isHeal = false)
    {
        Color color;
        if (isHeal)
            color = Color.LimeGreen;
        else if (isCritical)
            color = Color.Gold;
        else
            color = Color.White;

        // Slight random X offset to prevent stacking
        var rnd = new System.Random();
        float offsetX = (float)(rnd.NextDouble() * 0.4 - 0.2);
        Vector2 pos = worldPosition + new Vector2(offsetX, 0f);

        _popups.Add(new DamagePopup(pos, damage, color));
    }

    /// <summary>
    /// Updates all active popups and removes expired ones.
    /// </summary>
    public void Update(GameTime gameTime)
    {
        for (int i = _popups.Count - 1; i >= 0; i--)
        {
            _popups[i].Update(gameTime);
            if (!_popups[i].IsActive)
            {
                _popups.RemoveAt(i);
            }
        }
    }

    /// <summary>
    /// Draws all active popups.
    /// </summary>
    public void Draw(SpriteBatch spriteBatch, Camera2D camera)
    {
        foreach (var popup in _popups)
        {
            popup.Draw(spriteBatch, camera);
        }
    }
}
