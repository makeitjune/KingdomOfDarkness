using Microsoft.Xna.Framework;
using KingdomOfDarkness.Entities;
using KingdomOfDarkness.World;

namespace KingdomOfDarkness.Systems;

public class IsoMovementSystem
{
    public void Update(GameTime gameTime, Character character, CollisionMap collisionMap, System.Func<Vector2, bool> isOccupied = null)
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

            // Tap to turn: if changing direction, only change facing and apply full movement delay
            if (character.FacingDirection != intent)
            {
                character.FacingDirection = intent;
                character.MoveCooldownRemaining = 1f / character.MoveSpeed;
                character.MovementIntent = Vector2.Zero;
                return;
            }

            Vector2 nextPosition = character.WorldPosition + intent;

            bool canMove = true;
            
            if (collisionMap != null && collisionMap.IsBlocked(nextPosition.X, nextPosition.Y))
            {
                canMove = false;
            }
            
            if (isOccupied != null && isOccupied(nextPosition))
            {
                canMove = false;
            }

            if (canMove)
            {
                character.WorldPosition = nextPosition;
                // Trigger cooldown. For example, MoveSpeed = 3.5 means 3.5 tiles per second, so cooldown = 1 / 3.5
                character.MoveCooldownRemaining = 1f / character.MoveSpeed;
            }

            // Reset intent after applying
            character.MovementIntent = Vector2.Zero;
        }
    }
}
