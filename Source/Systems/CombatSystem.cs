using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using KingdomOfDarkness.Entities;
using KingdomOfDarkness.UI;

namespace KingdomOfDarkness.Systems;

public class CombatSystem
{
    private readonly LevelSystem _levelSystem;
    private readonly DamagePopupManager _damagePopupManager;

    public CombatSystem(LevelSystem levelSystem, DamagePopupManager damagePopupManager)
    {
        _levelSystem = levelSystem;
        _damagePopupManager = damagePopupManager;
    }

    public void Update(GameTime gameTime, Player player, List<Companion> companions, List<Monster> monsters, bool attackRequested, DialogueReactionSystem dialogueSystem)
    {
        // 1. Target selection: If player has no target or target is dead, select the nearest alive monster
        if (player.Target == null || player.Target.IsDead)
        {
            player.Target = FindClosestMonster(player.WorldPosition, monsters);
        }

        // 2. Companion targets same monster as player if assisting
        foreach (var companion in companions)
        {
            if (companion.State == CompanionState.AssistAttack)
            {
                companion.Target = player.Target;
            }
        }

        // 3. Process Player attacks
        if (player.Target != null && !player.Target.IsDead)
        {
            // Player attacks when Space (attackRequested) is pressed/held AND within range
            if (attackRequested)
            {
                TryAttack(player, player.Target, player, companions, dialogueSystem);
            }
        }

        // 4. Process Companion attacks (attacks automatically if state is AssistAttack and target is in range)
        foreach (var companion in companions)
        {
            if (companion.Target != null && !companion.Target.IsDead && companion.State == CompanionState.AssistAttack)
            {
                TryAttack(companion, companion.Target, player, companions, dialogueSystem);
            }
        }

        // 5. Process Monster attacks (attacks target player/companion automatically when in range)
        foreach (var monster in monsters)
        {
            if (monster.Target != null && !monster.Target.IsDead && monster.State == MonsterState.Attack)
            {
                TryAttack(monster, monster.Target, player, companions, dialogueSystem);
            }
        }
    }

    private Monster FindClosestMonster(Vector2 position, List<Monster> monsters)
    {
        Monster closest = null;
        float minDist = float.MaxValue;

        foreach (var monster in monsters)
        {
            if (monster.IsDead) continue;

            float dist = Vector2.Distance(position, monster.WorldPosition);
            if (dist < minDist)
            {
                minDist = dist;
                closest = monster;
            }
        }

        return closest;
    }

    private void TryAttack(Character attacker, Character target, Player player, List<Companion> companions, DialogueReactionSystem dialogueSystem)
    {
        if (attacker == null || target == null || attacker.IsDead || target.IsDead) return;

        float distance = Vector2.Distance(attacker.WorldPosition, target.WorldPosition);
        
        // Check range and cooldown
        if (distance <= attacker.AttackRange && attacker.AttackCooldownRemaining <= 0f)
        {
            // Damage formula: damage = max(1, attacker.AttackPower - target.Defense)
            int damage = Math.Max(1, attacker.AttackPower - target.Defense);
            target.CurrentHP -= damage;

            // Reset cooldown
            attacker.AttackCooldownRemaining = attacker.AttackCooldownSeconds;

            // Spawn damage popup
            _damagePopupManager.Spawn(target.WorldPosition, damage);

            // Check for death
            if (target.IsDead)
            {
                target.CurrentHP = 0;

                // Award EXP if target was a Monster
                if (target is Monster monster)
                {
                    player.Experience += monster.ExperienceReward;
                    _levelSystem.CheckLevelUp(player);

                    foreach (var comp in companions)
                    {
                        if (!comp.IsDead)
                        {
                            comp.Experience += monster.ExperienceReward;
                            _levelSystem.CheckLevelUp(comp);

                            // Trigger dialogue reaction on kill
                            dialogueSystem.TriggerReaction(comp, comp.Bubble, DialogueEvent.KillMonster);
                        }
                    }
                }
            }
        }
    }
}
