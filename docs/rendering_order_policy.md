# Rendering Order Policy

## Purpose

Quarter-view games need correct front/back drawing.

If an entity is visually lower on the screen, it should usually draw over an entity behind it.

## Draw Layers

Recommended basic draw order:

```text
1. Ground tiles
2. Ground overlays
3. Entities sorted by depth
4. Effects
5. UI
```

## Entity Sort Key

First MVP sort key:

```csharp
sortKey = entity.WorldPosition.X + entity.WorldPosition.Y;
```

Sort ascending and draw in that order.

Entities with larger `X + Y` draw later.

## Tie Breakers

If two entities have same sort key:

1. `DrawLayer`
2. `WorldPosition.Y`
3. `Id`

Suggested properties:

```csharp
public float DrawLayerOffset { get; set; }
public int Id { get; }
```

Do not overbuild this in MVP.

## Sprite Origin

Character sprite origin should represent feet/base point.

For placeholder rectangles:

```text
originX = width / 2
originY = height
```

This makes `WorldPosition` correspond to the bottom center of the character.

## Tile Drawing

Tiles are drawn before entities.

Tile sort can be simple nested loops for MVP.

## Common Mistakes

- Drawing all entities before tiles.
- Drawing entities in creation order.
- Sorting by raw texture top-left Y.
- Sorting by screen Y while using inconsistent sprite origins.
- Letting UI participate in world render sorting.
