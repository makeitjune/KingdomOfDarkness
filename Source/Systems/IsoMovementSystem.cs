using Microsoft.Xna.Framework;
using KingdomOfDarkness.Entities;
using KingdomOfDarkness.World;

namespace KingdomOfDarkness.Systems;

public class IsoMovementSystem
{
    public void Update(GameTime gameTime, Character character, CollisionMap collisionMap)
    {
        if (character == null || character.IsDead) return;

        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

        // Apply velocity to position
        if (character.Velocity != Vector2.Zero)
        {
            Vector2 nextPosition = character.WorldPosition + character.Velocity * dt;

            if (collisionMap != null)
            {
                // Verify if full diagonal movement is clear
                if (!collisionMap.IsBlocked(nextPosition.X, nextPosition.Y))
                {
                    character.WorldPosition = nextPosition;
                }
                else
                {
                    // Slide check: try to move only along X-axis
                    Vector2 nextX = character.WorldPosition + new Vector2(character.Velocity.X, 0f) * dt;
                    if (!collisionMap.IsBlocked(nextX.X, nextX.Y))
                    {
                        character.WorldPosition = nextX;
                    }
                    else
                    {
                        // Slide check: try to move only along Y-axis
                        Vector2 nextY = character.WorldPosition + new Vector2(0f, character.Velocity.Y) * dt;
                        if (!collisionMap.IsBlocked(nextY.X, nextY.Y))
                        {
                            character.WorldPosition = nextY;
                        }
                        else
                        {
                            // Completely blocked: halt character velocity
                            character.Velocity = Vector2.Zero;
                        }
                    }
                }
            }
            else
            {
                character.WorldPosition = nextPosition;
            }
        }
    }
}
