# Companion AI Design

## Goal

The companion should feel like a party member, not a pet that teleports or overlaps the player.

MVP companion behavior:

- Follow player.
- Keep comfortable distance.
- Assist in combat.
- Retreat when low HP.
- Speak with simple rule-based reactions.

## Companion State Machine

Initial states:

```text
Idle
FollowPlayer
AssistAttack
Retreat
Dead
```

Optional later states:

```text
Stuck
BlockedByPlayer
UseSkill
HealAlly
```

## Follow Distance

Use world-space distance.

Suggested thresholds:

```text
TooCloseDistance      = 0.6
ComfortMinDistance   = 0.8
ComfortMaxDistance   = 1.8
TooFarDistance        = 4.0
TeleportDistance      = disabled for MVP
```

First rule:

```text
if distance > ComfortMaxDistance:
    follow player
else:
    idle
```

Do not move to the exact player coordinate.

## Formation Offset

A simple first formation target:

```text
target = player.WorldPosition + new Vector2(-0.8f, 0.8f)
```

This places companion slightly behind/side depending on transform.

Later, formation can depend on player facing.

## Assist Combat

Initial rule:

```text
if player has target and target is alive:
    companion target = player target
    state = AssistAttack
else if hostile monster nearby:
    target nearest hostile
else:
    follow player
```

## Low HP Retreat

Initial rule:

```text
if companion HP percentage < 30%:
    state = Retreat
```

Retreat behavior for MVP:

- Move away from current target for a short distance.
- Do not require smart pathfinding yet.

## Speech Events

Companion can request speech events:

```text
CombatStart
LowHealth
TooFar
KillMonster
BlockedByPlayer
```

`DialogueReactionSystem` decides text and cooldown.

## Anti-overlap Rule

Do not constantly set:

```csharp
companion.WorldPosition = player.WorldPosition;
```

This is forbidden.

Move toward a desired formation point.

## Manual Test Checklist

- Companion follows when player moves away.
- Companion stops near the player.
- Companion does not always stand inside player.
- Companion helps attack player's target.
- Companion can show a speech bubble.
