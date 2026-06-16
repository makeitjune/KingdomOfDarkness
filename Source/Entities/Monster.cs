using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KingdomOfDarkness.Entities;

public enum MonsterState
{
    Idle,
    Chase,
    Attack,
    Dead
}

public class Monster : Character
{
    public MonsterState State { get; set; } = MonsterState.Idle;
    public int ExperienceReward { get; set; } = 25;
    public float AggroRange { get; set; } = 4.0f; // world units
    public float RespawnCooldownRemaining { get; set; } = 0f;
    private Vector2 _spawnPosition;

    public Monster(Texture2D whitePixel, Vector2 startWorldPosition)
        : base(
            whitePixel,
            "Goblin",
            50,  // MaxHP
            8,   // AttackPower
            1,   // Defense
            2.0f // MoveSpeed
        )
    {
        WorldPosition = startWorldPosition;
        _spawnPosition = startWorldPosition;
        DebugColor = Color.OrangeRed; // Hostile orange-red color
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        if (IsDead)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (RespawnCooldownRemaining <= 0f)
            {
                RespawnCooldownRemaining = 5.0f; // 5 seconds respawn timer
            }
            else
            {
                RespawnCooldownRemaining -= dt;
                if (RespawnCooldownRemaining <= 0f)
                {
                    // Respawn the monster
                    CurrentHP = MaxHP;
                    State = MonsterState.Idle;
                    // Slightly randomize respawn position around spawn area
                    var rnd = new System.Random();
                    WorldPosition = _spawnPosition + new Vector2(
                        (float)(rnd.NextDouble() * 2.0 - 1.0),
                        (float)(rnd.NextDouble() * 2.0 - 1.0)
                    );
                    Target = null;
                }
            }
        }
    }
}
