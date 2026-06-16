# Phase 2 Task — Quarter-view Test Map

## Goal

Render a visible quarter-view diamond tile test map.

## Must Read

- `docs/iso_coordinate_system.md`
- `docs/rendering_order_policy.md`
- `docs/content_pipeline.md`

## Create

```text
Source/World/IsoTile.cs
Source/World/IsoTileMap.cs
```

## Required Behavior

- Render at least a 10x10 tile map.
- Tiles must appear as diamonds.
- Use `IsoMath.WorldToScreen()`.
- Apply camera offset.
- Use placeholder generated texture or primitive drawing.
- No external art required.

## Suggested Implementation

Create a 1x1 white texture in `Game1.cs`, then use `SpriteBatch.Draw` with generated diamond texture or line/filled approximation.

If generating a diamond texture:

- Width: `GameConstants.TileWidth`
- Height: `GameConstants.TileHeight`
- Fill pixels inside diamond shape.
- Draw at screen position with center origin.

## Do Not

- Do not draw square top-down tiles.
- Do not implement player movement in this phase unless it already exists.
- Do not add full map loader.

## Validation

```powershell
dotnet build
dotnet run
```

## Manual Check

- Tiles look like diamond/isometric tiles.
- Map rows are diagonal.
- The result does not look like a flat square top-down grid.
