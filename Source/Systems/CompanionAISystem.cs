using Microsoft.Xna.Framework;
using KingdomOfDarkness.Entities;

namespace KingdomOfDarkness.Systems;

public class CompanionAISystem
{
    private const float TooCloseDistance = 0.6f;
    private const float ComfortMinDistance = 0.8f;
    private const float ComfortMaxDistance = 1.8f;
    private const float RetreatThreshold = 0.3f; // 30% HP

    public void Update(GameTime gameTime, Companion companion, Player player, DialogueReactionSystem dialogueSystem)
    {
        if (companion == null || companion.IsDead)
        {
            if (companion != null)
            {
                companion.State = CompanionState.Dead;
                companion.Velocity = Vector2.Zero;
            }
            return;
        }

        float distToPlayer = Vector2.Distance(companion.WorldPosition, player.WorldPosition);
        CompanionState oldState = companion.State;

        // State Transitions
        // 1. Low HP Retreat
        if ((float)companion.CurrentHP / companion.MaxHP < RetreatThreshold)
        {
            companion.State = CompanionState.Retreat;
            if (oldState != CompanionState.Retreat)
            {
                dialogueSystem.TriggerReaction(companion, companion.Bubble, DialogueEvent.LowHealth);
            }
        }
        // 2. Assist in combat if player is targeting an alive monster
        else if (player.Target != null && !player.Target.IsDead)
        {
            companion.Target = player.Target;
            companion.State = CompanionState.AssistAttack;
            if (oldState != CompanionState.AssistAttack)
            {
                dialogueSystem.TriggerReaction(companion, companion.Bubble, DialogueEvent.CombatStart);
            }
        }
        // 3. Fall back to follow player if too far
        else if (distToPlayer > ComfortMaxDistance)
        {
            companion.State = CompanionState.FollowPlayer;
            if (oldState != CompanionState.FollowPlayer && distToPlayer > 2.8f)
            {
                dialogueSystem.TriggerReaction(companion, companion.Bubble, DialogueEvent.TooFar);
            }
        }
        // 4. Idle if within comfort range
        else if (distToPlayer < ComfortMinDistance && companion.State == CompanionState.FollowPlayer)
        {
            companion.State = CompanionState.Idle;
        }

        // State Behaviors
        switch (companion.State)
        {
            case CompanionState.Idle:
                companion.Velocity = Vector2.Zero;
                // If player moves too far again, transition back to follow
                if (distToPlayer > ComfortMaxDistance)
                {
                    companion.State = CompanionState.FollowPlayer;
                }
                break;

            case CompanionState.FollowPlayer:
                // Move towards formation offset (slightly behind the player)
                Vector2 formationOffset = new Vector2(-0.8f, 0.8f);
                Vector2 targetPos = player.WorldPosition + formationOffset;
                float distToTarget = Vector2.Distance(companion.WorldPosition, targetPos);

                if (distToTarget > 0.15f)
                {
                    Vector2 dir = targetPos - companion.WorldPosition;
                    dir.Normalize();
                    companion.Velocity = dir * companion.MoveSpeed;
                }
                else
                {
                    companion.Velocity = Vector2.Zero;
                    companion.State = CompanionState.Idle;
                }
                break;

            case CompanionState.AssistAttack:
                if (companion.Target == null || companion.Target.IsDead)
                {
                    companion.Target = null;
                    companion.State = CompanionState.FollowPlayer;
                    break;
                }

                // Move within attack range of the target
                float distToMonster = Vector2.Distance(companion.WorldPosition, companion.Target.WorldPosition);
                if (distToMonster > companion.AttackRange)
                {
                    Vector2 dirToMonster = companion.Target.WorldPosition - companion.WorldPosition;
                    dirToMonster.Normalize();
                    companion.Velocity = dirToMonster * companion.MoveSpeed;
                }
                else
                {
                    // In range: stop and wait to attack (handled by CombatSystem)
                    companion.Velocity = Vector2.Zero;
                }
                break;

            case CompanionState.Retreat:
                // If HP recovers somehow, clear retreat (e.g. above 40%)
                if ((float)companion.CurrentHP / companion.MaxHP >= 0.4f)
                {
                    companion.State = CompanionState.FollowPlayer;
                    break;
                }

                // Move away from nearest threat (target or player if no target, or player target)
                Character threat = companion.Target ?? player.Target;
                if (threat != null && !threat.IsDead)
                {
                    Vector2 runDir = companion.WorldPosition - threat.WorldPosition;
                    if (runDir.LengthSquared() > 0.001f)
                    {
                        runDir.Normalize();
                        companion.Velocity = runDir * companion.MoveSpeed;
                    }
                }
                else
                {
                    // If no threat, run towards player but keep distance
                    if (distToPlayer > ComfortMinDistance)
                    {
                        Vector2 dirToPlayer = player.WorldPosition - companion.WorldPosition;
                        dirToPlayer.Normalize();
                        companion.Velocity = dirToPlayer * companion.MoveSpeed;
                    }
                    else
                    {
                        companion.Velocity = Vector2.Zero;
                    }
                }
                break;

            case CompanionState.Dead:
                companion.Velocity = Vector2.Zero;
                break;
        }
    }
}
