# Collision Policy

## MVP Timing

Do not implement collision before:

1. Quarter-view tile map renders.
2. Player moves correctly.
3. Companion follows.
4. Combat prototype exists.

Collision can be Phase 10.

## Coordinate Rule

Collision must use world/tile coordinates.

Do not use screen pixel rectangles for gameplay map collision.

## First Collision Model

Use tile blocking:

```text
0 = walkable
1 = blocked
```

Character target position:

```csharp
Vector2 nextPosition = currentPosition + movement * speed * deltaSeconds;
Point tile = IsoMath.WorldToTile(nextPosition);
if (!collisionMap.IsBlocked(tile.X, tile.Y))
{
    currentPosition = nextPosition;
}
```

## Entity Collision

For MVP, entity collision can be soft:

- Player and companion may overlap slightly.
- Companion AI should avoid exact same target point.
- Monster collision can be ignored until core combat works.

## Future

Later collision improvements:

- Character radius.
- Separation steering.
- Pathfinding around blocked tiles.
- Player body blocking and companion “blocked” reactions.
