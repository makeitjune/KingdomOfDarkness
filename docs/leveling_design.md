# Leveling Design

## MVP Goal

Add a small RPG growth loop without building a full character progression system.

## Minimum Data

```csharp
public int Level { get; set; } = 1;
public int Experience { get; set; } = 0;
public int ExperienceToNextLevel { get; set; } = 100;
```

## First EXP Table

Simple formula:

```text
ExpToNext = 100 * Level
```

Examples:

```text
Level 1 → 2: 100 EXP
Level 2 → 3: 200 EXP
Level 3 → 4: 300 EXP
```

## Level Up Reward

MVP stat increase:

```text
MaxHP + 10
AttackPower + 2
Defense + 1 every 2 levels
CurrentHP = MaxHP
```

## Companion EXP

For MVP:

- Player and companion both receive full EXP when fighting together.

Later:

- Split EXP.
- Party range check.
- Dead companion rules.

## Acceptance Criteria

- EXP number increases after monster death.
- Level increases when threshold is reached.
- Stat increase is visible in debug/HUD/log.
- EXP is not awarded repeatedly from same dead monster.
