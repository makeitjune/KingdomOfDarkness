# Combat System Design

## MVP Combat Goal

Create a tiny but complete hunting loop:

```text
Find monster → attack → HP decreases → monster dies → EXP gained
```

## Character Combat Data

Minimum properties:

```csharp
public int Level { get; set; }
public int CurrentHP { get; set; }
public int MaxHP { get; set; }
public int AttackPower { get; set; }
public int Defense { get; set; }
public float AttackRange { get; set; }
public float AttackCooldownSeconds { get; set; }
public float AttackCooldownRemaining { get; set; }
public bool IsDead => CurrentHP <= 0;
```

## Combat Flow

```text
1. Select target.
2. Check attacker alive.
3. Check target alive.
4. Check distance.
5. Check cooldown.
6. Apply damage.
7. Reset cooldown.
8. If target dies, publish death/EXP event.
```

## Distance

Use world-space distance:

```csharp
float distance = Vector2.Distance(attacker.WorldPosition, target.WorldPosition);
```

Do not use screen-space distance for combat.

## Damage Formula

MVP formula:

```text
damage = max(1, attacker.AttackPower - target.Defense)
```

Later:

```text
damage = base + random variance + critical + skill modifier - defense
```

Do not add later formula in MVP.

## Targeting

First version:

- Player target can be nearest monster in range or selected monster.
- Companion target follows player target.
- Monster target can be nearest player-side character.

## Monster AI

Initial states:

```text
Idle
Chase
Attack
Dead
```

Rules:

```text
if dead:
    Dead
else if target in attack range:
    Attack
else if target in aggro range:
    Chase
else:
    Idle
```

## EXP Reward

Monster has:

```csharp
public int ExperienceReward { get; set; }
```

On death:

```text
Player gains EXP.
Companion may gain same EXP or partial EXP.
```

For MVP, give both full EXP.

## Manual Test Checklist

- Player can damage monster.
- Monster HP reaches 0.
- Dead monster stops attacking.
- Companion can damage monster.
- Monster can damage player/companion.
- EXP is granted once, not every frame after death.
