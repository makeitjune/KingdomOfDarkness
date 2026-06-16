# Phase 4 Task — Camera and Render Order

## Goal

Make camera follow the player and render entities in quarter-view depth order.

## Must Read

- `docs/iso_coordinate_system.md`
- `docs/rendering_order_policy.md`

## Create

```text
Source/Systems/RenderOrderSystem.cs
```

## Required Behavior

- Camera follows player.
- Entity draw order uses world position.
- First sort key: `WorldPosition.X + WorldPosition.Y`.

## Do Not

- Do not store camera offset in entity world positions.
- Do not use creation order for entity draw order.
- Do not implement combat.
- Do not implement pathfinding.

## Validation

```powershell
dotnet build
dotnet run
```

## Manual Check

- Player remains near center while moving.
- If test entities exist, lower-screen entity draws in front.
