using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using KingdomOfDarkness.Core;

namespace KingdomOfDarkness.Entities;

public class Player : Character
{
    public Player(Texture2D whitePixel, Vector2 startWorldPosition)
        : base(
            whitePixel,
            "Player",
            100, // MaxHP
            15,  // AttackPower
            5,   // Defense
            GameConstants.DefaultPlayerMoveSpeed
        )
    {
        WorldPosition = startWorldPosition;
        DebugColor = Color.CornflowerBlue; // Blue-ish character placeholder
    }

    public void UpdateInput(InputManager inputManager)
    {
        if (IsDead)
        {
            Velocity = Vector2.Zero;
            return;
        }

        // Set velocity based on input movement intent
        Velocity = inputManager.MovementIntent * MoveSpeed;
    }
}
