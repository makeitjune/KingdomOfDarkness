using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using KingdomOfDarkness.Core;
using KingdomOfDarkness.Data;

namespace KingdomOfDarkness.Entities;

public class Player : Character
{
    private Vector2 _startPosition;
    public float RespawnCooldownRemaining { get; set; } = 0f;

    public Player(Texture2D whitePixel, Vector2 startWorldPosition, CharacterClassType classType)
        : base(
            whitePixel,
            "아레스", // Player's name
            ClassDatabase.GetClass(classType)
        )
    {
        WorldPosition = startWorldPosition;
        _startPosition = startWorldPosition;
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        if (IsDead)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (RespawnCooldownRemaining <= 0f)
            {
                RespawnCooldownRemaining = 5.0f; // 5 seconds respawn
            }
            else
            {
                RespawnCooldownRemaining -= dt;
                if (RespawnCooldownRemaining <= 0f)
                {
                    // Respawn
                    CurrentHP = MaxHP / 2; // Respawn with 50% HP
                    WorldPosition = _startPosition;
                }
            }
        }
    }

    public void UpdateInput(InputManager inputManager)
    {
        if (IsDead)
        {
            MovementIntent = Vector2.Zero;
            return;
        }

        // Prevent moving shot: if attacking, stop moving
        if (inputManager.IsAttackRequested)
        {
            MovementIntent = Vector2.Zero;
        }
        else
        {
            MovementIntent = inputManager.MovementIntent;
        }
    }
}
