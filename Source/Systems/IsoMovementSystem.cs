using Microsoft.Xna.Framework;
using KingdomOfDarkness.Entities;
using KingdomOfDarkness.World;

namespace KingdomOfDarkness.Systems;

public class IsoMovementSystem
{
    public void Update(GameTime gameTime, Character character, CollisionMap collisionMap)
    {
        if (character == null || character.IsDead) return;

        // If cooldown is active, cannot move
        if (character.MoveCooldownRemaining > 0f) return;

        if (character.MovementIntent != Vector2.Zero)
        {
            // Normalize just in case, though it should already be orthogonal
            Vector2 intent = character.MovementIntent;
            
            // Strictly enforce single axis movement
            if (System.Math.Abs(intent.X) > 0 && System.Math.Abs(intent.Y) > 0)
            {
                // Prefer X if both are somehow set
                intent.Y = 0;
            }

            // Ensure magnitude is 1
            if (intent.X > 0) intent.X = 1;
            if (intent.X < 0) intent.X = -1;
            if (intent.Y > 0) intent.Y = 1;
            if (intent.Y < 0) intent.Y = -1;

            Vector2 nextPosition = character.WorldPosition + intent;

            if (collisionMap != null)
            {
                if (!collisionMap.IsBlocked(nextPosition.X, nextPosition.Y))
                {
                    character.WorldPosition = nextPosition;
                    // Trigger cooldown. For example, MoveSpeed = 3.5 means 3.5 tiles per second, so cooldown = 1 / 3.5
                    character.MoveCooldownRemaining = 1f / character.MoveSpeed;
                }
            }
            else
            {
                character.WorldPosition = nextPosition;
                character.MoveCooldownRemaining = 1f / character.MoveSpeed;
            }

            // Reset intent after applying
            character.MovementIntent = Vector2.Zero;
        }
    }
}
