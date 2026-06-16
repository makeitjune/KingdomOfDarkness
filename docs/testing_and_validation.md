# Testing and Validation

## Basic Commands

Run after code changes:

```powershell
dotnet build
```

Run after gameplay/visual changes:

```powershell
dotnet run
```

## Manual Visual Validation

### Quarter-view Map

- Tiles appear as diamonds.
- Grid is not square top-down.
- Tile rows form diagonal lines.

### Player Movement

- W/A/S/D move on screen diagonals.
- Movement is not raw top-down up/down/left/right.
- Movement speed is stable across frame rates.

### Camera

- Camera follows player.
- World positions are not modified by camera.

### Render Order

- Lower entities draw in front.
- Higher/back entities draw behind.

### Companion

- Companion follows player.
- Companion keeps distance.
- Companion does not constantly overlap player.

### Combat

- Attack only happens in range.
- HP decreases.
- Death stops actions.
- EXP is awarded once.

### Speech

- Speech bubble appears.
- Text disappears after timeout.
- Text does not spam every frame.

## Suggested Future Automated Tests

Later, add a test project:

```text
KingdomOfDarkness.Tests/
```

Potential unit tests:

- `IsoMath.WorldToScreen`
- `IsoMath.ScreenToWorldApprox`
- EXP level thresholds
- Damage formula
- Render sort key
- Companion distance state selection

Do not block early MVP on test project setup unless requested.
