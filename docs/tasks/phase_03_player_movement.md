# Phase 3 Task — Player Movement

## Goal

Add a player entity that moves diagonally on the quarter-view map.

## Must Read

- `docs/iso_coordinate_system.md`
- `docs/movement_and_controls.md`
- `docs/rendering_order_policy.md`

## Create

```text
Source/Entities/Entity.cs
Source/Entities/Character.cs
Source/Entities/Player.cs
Source/Systems/IsoMovementSystem.cs
```

## Required Behavior

- Player stores `WorldPosition`.
- Player movement uses `InputManager.MovementIntent`.
- Player movement uses delta time.
- Player draw position is computed with `IsoMath.WorldToScreen`.
- Player appears anchored by feet/base point.
- Player moves diagonally on screen.

## Placeholder Rendering

Use a simple rectangle or generated texture for player.

Suggested visual:

```text
Player = blue-ish or distinct placeholder rectangle
```

Do not rely on final art.

## Do Not

- Do not move player by raw screen pixels.
- Do not implement full combat.
- Do not implement companion in this phase unless explicitly asked.
- Do not use top-down movement.

## Validation

```powershell
dotnet build
dotnet run
```

## Manual Check

- W/A/S/D move the player along screen diagonals.
- Movement speed is not faster when two keys are held.
- Map remains quarter-view.
