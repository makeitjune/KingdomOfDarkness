using Microsoft.Xna.Framework;
using KingdomOfDarkness.Entities;

namespace KingdomOfDarkness.Systems;

public class MonsterAISystem
{
    public void Update(GameTime gameTime, Monster monster, Player player, Companion companion)
    {
        if (monster == null) return;

        if (monster.IsDead)
        {
            monster.State = MonsterState.Dead;
            monster.Velocity = Vector2.Zero;
            monster.Target = null;
            return;
        }

        // Target selection: Target the nearest alive player-side character
        Character potentialTarget = null;
        float distToPlayer = player.IsDead ? float.MaxValue : Vector2.Distance(monster.WorldPosition, player.WorldPosition);
        float distToCompanion = companion.IsDead ? float.MaxValue : Vector2.Distance(monster.WorldPosition, companion.WorldPosition);

        if (distToPlayer < distToCompanion)
        {
            if (!player.IsDead && distToPlayer <= monster.AggroRange)
                potentialTarget = player;
        }
        else
        {
            if (!companion.IsDead && distToCompanion <= monster.AggroRange)
                potentialTarget = companion;
        }

        monster.Target = potentialTarget;

        if (monster.Target == null)
        {
            monster.State = MonsterState.Idle;
            monster.Velocity = Vector2.Zero;
            return;
        }

        float distance = Vector2.Distance(monster.WorldPosition, monster.Target.WorldPosition);

        // State Machine
        if (distance <= monster.AttackRange)
        {
            monster.State = MonsterState.Attack;
            monster.Velocity = Vector2.Zero;
        }
        else
        {
            monster.State = MonsterState.Chase;
            Vector2 dir = monster.Target.WorldPosition - monster.WorldPosition;
            if (dir.LengthSquared() > 0.001f)
            {
                dir.Normalize();
                monster.Velocity = dir * monster.MoveSpeed;
            }
            else
            {
                monster.Velocity = Vector2.Zero;
            }
        }
    }
}
