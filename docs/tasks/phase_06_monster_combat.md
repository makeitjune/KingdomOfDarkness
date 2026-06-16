# Phase 6 Task — Monster and Combat

## Goal

Add one monster and a simple combat loop.

## Must Read

- `docs/combat_system_design.md`
- `docs/companion_ai_design.md`
- `docs/leveling_design.md`

## Create

```text
Source/Entities/Monster.cs
Source/Systems/CombatSystem.cs
Source/Systems/MonsterAISystem.cs
```

## Required Behavior

- Monster exists on map.
- Player can attack monster.
- Companion can assist attack.
- Monster can attack player or companion.
- HP decreases.
- Death stops actions.
- EXP is granted once.

## Combat Rules

Use MVP rules from `docs/combat_system_design.md`.

## Do Not

- Do not add full skill system.
- Do not add equipment.
- Do not add advanced effects.
- Do not use screen distance for attack range.

## Validation

```powershell
dotnet build
dotnet run
```

## Manual Check

- Player and companion can kill monster.
- Dead monster stops attacking.
- EXP increases.
