using Microsoft.Xna.Framework;
using KingdomOfDarkness.Entities;

namespace KingdomOfDarkness.Systems;

public class IsoMovementSystem
{
    public void Update(GameTime gameTime, Character character)
    {
        if (character == null || character.IsDead) return;

        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

        // Apply velocity to position
        if (character.Velocity != Vector2.Zero)
        {
            character.WorldPosition += character.Velocity * dt;
        }
    }
}
