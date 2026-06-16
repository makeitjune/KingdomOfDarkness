# Movement and Controls

## Design Target

Movement must feel like old quarter-view 2D RPG movement.

The player should move diagonally on screen, not in top-down cardinal screen directions.

## Default Input

Keyboard:

```text
W = move toward upper-right screen diagonal
S = move toward lower-left screen diagonal
A = move toward upper-left screen diagonal
D = move toward lower-right screen diagonal
```

Internal first mapping:

```text
W => worldY - 1
S => worldY + 1
A => worldX - 1
D => worldX + 1
```

This mapping may be tuned after visual testing.

## InputManager Responsibility

`InputManager` should:

- Read keyboard state.
- Produce movement intent as a `Vector2` in world-direction space.
- Normalize when needed.
- Hide MonoGame keyboard details from `Player`.

Example output:

```csharp
public Vector2 MovementIntent { get; }
```

## Player Responsibility

`Player` should:

- Receive movement intent.
- Apply speed and delta time.
- Ask collision system if movement is allowed when collision exists.
- Store final world position.

## Movement Formula

```csharp
float deltaSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
WorldPosition += movementIntent * MoveSpeed * deltaSeconds;
```

## Speed Defaults

Initial values:

```text
Player move speed: 3.5 world units/sec
Companion move speed: 3.2 world units/sec
Monster move speed: 2.3 world units/sec
```

Tune later by feel.

## Direction and Facing

For MVP, facing direction can be based on movement intent.

Possible enum:

```csharp
public enum IsoDirection
{
    NorthEast,
    NorthWest,
    SouthEast,
    SouthWest
}
```

Do not block MVP on animation-facing correctness.

## Collision Timing

Collision should not be implemented before basic movement feels right.

Recommended order:

1. No collision.
2. Add map boundary.
3. Add blocked tiles.
4. Add entity separation.
5. Add pathfinding later.

## Manual Test Checklist

- Press W: character moves on a screen diagonal, not straight up.
- Press S: opposite of W.
- Press A: different screen diagonal.
- Press D: opposite of A.
- Holding two movement keys does not make movement too fast.
- Player world position changes, not only screen position.
