# Phase 5 Task — Companion Follow

## Goal

Add one companion that follows the player with distance control.

## Must Read

- `docs/companion_ai_design.md`
- `docs/iso_coordinate_system.md`
- `docs/movement_and_controls.md`

## Create

```text
Source/Entities/Companion.cs
Source/Systems/CompanionAISystem.cs
```

## Required Behavior

- Companion has world position.
- Companion follows player when too far.
- Companion stops at comfortable distance.
- Companion does not constantly overlap player.
- Companion renders with correct depth order.

## Follow Distances

Use values from `docs/companion_ai_design.md` unless there is a clear reason to tune.

## Do Not

- Do not teleport companion every frame.
- Do not add 4 companions yet.
- Do not implement pathfinding yet.
- Do not implement chatbot behavior.

## Validation

```powershell
dotnet build
dotnet run
```

## Manual Check

- Move player away: companion follows.
- Stop player: companion approaches and stops nearby.
- Companion remains on quarter-view map and draws correctly.
