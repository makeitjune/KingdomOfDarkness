# Phase 8 Task — Collision

## Goal

Add basic blocked tile collision after movement/combat is working.

## Must Read

- `docs/collision_policy.md`
- `docs/iso_coordinate_system.md`

## Create

```text
Source/World/CollisionMap.cs
```

## Required Behavior

- Some tiles can be blocked.
- Player cannot move into blocked tiles.
- Collision uses world/tile coordinates.
- Companion follow remains usable.

## Do Not

- Do not use screen-space collision.
- Do not implement complex pathfinding yet.
- Do not implement physics engine behavior.

## Validation

```powershell
dotnet build
dotnet run
```

## Manual Check

- Player cannot walk through blocked tiles.
- Movement still feels quarter-view.
