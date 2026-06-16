using System.Collections.Generic;
using Microsoft.Xna.Framework;
using KingdomOfDarkness.Entities;

namespace KingdomOfDarkness.Systems;

public class MonsterAISystem
{
    public void Update(GameTime gameTime, Monster monster, Player player, List<Companion> companions)
    {
        if (monster == null) return;

        if (monster.IsDead)
        {
            monster.State = MonsterState.Dead;
            monster.MovementIntent = Vector2.Zero;
            monster.Target = null;
            return;
        }

        // Target selection: Target the nearest alive player-side character
        Character potentialTarget = null;
        float minDist = float.MaxValue;

        // Check player
        if (!player.IsDead)
        {
            float dist = Vector2.Distance(monster.WorldPosition, player.WorldPosition);
            if (dist <= monster.AggroRange && dist < minDist)
            {
                minDist = dist;
                potentialTarget = player;
            }
        }

        // Check companions
        foreach (var companion in companions)
        {
            if (!companion.IsDead)
            {
                float dist = Vector2.Distance(monster.WorldPosition, companion.WorldPosition);
                if (dist <= monster.AggroRange && dist < minDist)
                {
                    minDist = dist;
                    potentialTarget = companion;
                }
            }
        }

        monster.Target = potentialTarget;

        if (monster.Target == null)
        {
            monster.State = MonsterState.Idle;
            monster.MovementIntent = Vector2.Zero;
            return;
        }

        float distance = Vector2.Distance(monster.WorldPosition, monster.Target.WorldPosition);

        // Check if orthogonally adjacent (distance is exactly 1 on one axis)
        Vector2 diff = monster.Target.WorldPosition - monster.WorldPosition;
        bool isOrthogonallyAdjacent = (System.Math.Abs(diff.X) + System.Math.Abs(diff.Y)) == 1.0f;

        // State Machine
        if (isOrthogonallyAdjacent)
        {
            monster.State = MonsterState.Attack;
            monster.MovementIntent = Vector2.Zero;
            Vector2 dir = monster.Target.WorldPosition - monster.WorldPosition;
            if (dir != Vector2.Zero)
            {
                monster.FacingDirection = GetGridIntent(dir);
            }
        }
        else
        {
            monster.State = MonsterState.Chase;
            Vector2 dir = monster.Target.WorldPosition - monster.WorldPosition;
            if (dir.LengthSquared() > 0.001f)
            {
                monster.MovementIntent = GetGridIntent(dir);
            }
            else
            {
                monster.MovementIntent = Vector2.Zero;
            }
        }
    }

    private Vector2 GetGridIntent(Vector2 direction)
    {
        if (direction == Vector2.Zero) return Vector2.Zero;

        if (System.Math.Abs(direction.X) > System.Math.Abs(direction.Y))
        {
            return new Vector2(System.Math.Sign(direction.X), 0);
        }
        else
        {
            return new Vector2(0, System.Math.Sign(direction.Y));
        }
    }
}
