# Phase 1 Task — Core Helpers

## Goal

Create the core source structure and quarter-view math helpers.

## Must Read

- `AGENTS.md`
- `plan.md`
- `docs/architecture.md`
- `docs/iso_coordinate_system.md`
- `docs/movement_and_controls.md`

## Create

```text
Source/Core/GameConstants.cs
Source/Core/IsoMath.cs
Source/Core/InputManager.cs
Source/Core/Camera2D.cs
```

## Required Behavior

### GameConstants

Include:

```text
TileWidth = 64
TileHeight = 32
DefaultPlayerMoveSpeed = 3.5f
```

### IsoMath

Include:

```csharp
WorldToScreen(Vector2 world)
ScreenToWorldApprox(Vector2 screen)
```

Use the formulas from `docs/iso_coordinate_system.md`.

### InputManager

Read keyboard and expose movement intent in world-coordinate direction.

Initial mapping:

```text
W => worldY - 1
S => worldY + 1
A => worldX - 1
D => worldX + 1
```

Normalize if length squared > 1.

### Camera2D

Support:

```text
Position
ScreenCenter
WorldToCameraScreen()
FollowScreenPosition()
```

Exact method names can be adjusted, but camera must be separate from entity world positions.

## Modify

`Game1.cs` only enough to instantiate and use the helpers if needed.

## Do Not

- Do not implement combat.
- Do not implement companion.
- Do not implement tile map yet unless needed for visual debug.
- Do not implement top-down movement.

## Validation

```powershell
dotnet build
```

## Acceptance Criteria

- Project builds.
- Core files exist.
- IsoMath is available.
- InputManager is available.
- Camera2D is available.
