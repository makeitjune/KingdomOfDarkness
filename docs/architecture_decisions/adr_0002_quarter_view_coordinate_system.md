# ADR 0002 — Quarter-view Coordinate System

## Status

Accepted.

## Context

The intended game style is closer to old Korean 2D quarter-view RPGs such as 어둠의 전설 / 아스가르드.

A previous plan incorrectly described the game as top-down. That is rejected.

## Decision

The game uses a quarter-view / isometric-style coordinate system.

Gameplay uses world coordinates.

Rendering uses screen coordinates derived from:

```csharp
screenX = (worldX - worldY) * TileWidth / 2f;
screenY = (worldX + worldY) * TileHeight / 2f;
```

## Consequences

- Movement, collision, AI, and combat use world coordinates.
- Drawing converts world to screen.
- Camera operates as screen-space offset.
- Depth sorting uses world position.
- Player movement must feel diagonal on screen.

## Rejected

- Top-down square tile grid.
- Direct screen-pixel movement.
- Screen-space gameplay collision.
