# Quarter-view / Isometric Coordinate System

## Purpose

This document defines how world coordinates, screen coordinates, movement, rendering, and collision should work.

The game must feel like a **2D quarter-view RPG**, not a top-down game.

## Coordinate Types

Always keep two coordinate spaces separate.

```text
World coordinates:
- Used for gameplay logic.
- Used for movement, AI, distance, attack range, collision.
- Stored in entity state.

Screen coordinates:
- Used only for rendering.
- Computed from world coordinates.
- Affected by camera.
```

## Default Tile Size

```text
TileWidth  = 64
TileHeight = 32
```

This means one world tile appears as a 64x32 diamond.

## World-to-screen Transform

Use this as the default transform:

```csharp
screenX = (worldX - worldY) * TileWidth / 2f;
screenY = (worldX + worldY) * TileHeight / 2f;
```

Recommended implementation:

```csharp
public static Vector2 WorldToScreen(Vector2 world)
{
    float screenX = (world.X - world.Y) * GameConstants.TileWidth / 2f;
    float screenY = (world.X + world.Y) * GameConstants.TileHeight / 2f;
    return new Vector2(screenX, screenY);
}
```

## Screen-to-world Approximation

Useful for mouse picking later.

```csharp
worldX = screenY / TileHeight + screenX / TileWidth;
worldY = screenY / TileHeight - screenX / TileWidth;
```

Recommended implementation:

```csharp
public static Vector2 ScreenToWorldApprox(Vector2 screen)
{
    float worldX = screen.Y / GameConstants.TileHeight + screen.X / GameConstants.TileWidth;
    float worldY = screen.Y / GameConstants.TileHeight - screen.X / GameConstants.TileWidth;
    return new Vector2(worldX, worldY);
}
```

This does not include camera offset. Apply camera conversion before using it.

## Camera

Camera is a screen-space offset.

A common draw formula:

```csharp
Vector2 screen = IsoMath.WorldToScreen(worldPosition);
Vector2 drawPosition = screen - camera.Position + camera.ScreenCenter;
```

Do not modify entity world positions to move the camera.

## Movement Mapping

The player should feel like moving diagonally on screen.

Recommended first mapping:

```text
W = worldY - 1
S = worldY + 1
A = worldX - 1
D = worldX + 1
```

Under `WorldToScreen`, this creates diagonal screen movement.

Approximate screen result:

```text
W: up-right
S: down-left
A: up-left
D: down-right
```

If this feels reversed during manual testing, fix mapping in `InputManager` or `Player`, not in `IsoMath`.

## Alternative Named Directions

For internal logic, use world directions:

```text
NorthWest
NorthEast
SouthWest
SouthEast
```

or grid directions:

```text
WorldMinusX
WorldPlusX
WorldMinusY
WorldPlusY
```

Avoid confusing names like `Up`, `Down`, `Left`, `Right` unless they clearly mean screen direction.

## Movement Speed

Use world units per second.

Example:

```csharp
WorldPosition += direction * MoveSpeed * deltaSeconds;
```

Do not use raw pixels per second for gameplay movement.

## Normalization

When two keys are pressed together, normalize movement vector so diagonal input does not become faster unless intentionally designed.

```csharp
if (direction.LengthSquared() > 1f)
{
    direction.Normalize();
}
```

## Collision

Collision should use world/tile coordinates.

For MVP:

- Use simple tile blocking.
- Convert entity world position to tile coordinate.
- Check whether target tile is blocked.
- Do not use screen-space rectangle collision for map blocking.

## Entity Depth Sorting

For quarter-view, draw order should use world position.

First version:

```text
sortKey = worldX + worldY
```

Entities with larger `worldX + worldY` are lower on the screen and should usually draw later.

Add layer offsets later:

```text
sortKey = worldX + worldY + DrawLayerOffset
```

## Entity Anchor Point

For characters, the world position should represent the character's feet/base point, not the sprite's top-left corner.

Draw using origin offset:

```csharp
drawPosition = screenPosition - spriteFeetOrigin;
```

For placeholder rectangles, approximate the feet/base at the bottom center.

## Tile Anchor Point

A tile's world coordinate represents the center of its diamond.

If using a generated diamond texture, draw it around that center.

## Common Bugs

### Bug: Movement looks top-down

Cause:

- Directly changing screen X/Y.
- Drawing player without `IsoMath.WorldToScreen`.

Fix:

- Store and update `WorldPosition`.
- Convert to screen only during draw.

### Bug: Companion overlaps player constantly

Cause:

- No follow distance band.
- Moving directly to player's exact coordinate.

Fix:

- Use min/comfortable/max distance thresholds.

### Bug: Wrong front/back drawing

Cause:

- Entities drawn in list insertion order.
- Sorting by screen Y only without consistent anchor.

Fix:

- Sort by `WorldPosition.X + WorldPosition.Y`.

### Bug: Camera breaks mouse picking

Cause:

- Ignoring camera when converting screen to world.

Fix:

- Convert mouse screen coordinate to world-space screen coordinate before `ScreenToWorldApprox`.
