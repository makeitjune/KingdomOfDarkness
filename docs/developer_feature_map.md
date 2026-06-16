# Developer Feature Map

Use this file when deciding where to implement a feature.

## Golden Rule

If the feature touches quarter-view movement, map rendering, camera, collision, pathfinding, entity drawing, or AI position logic, read:

- `docs/iso_coordinate_system.md`
- `docs/rendering_order_policy.md`
- `docs/movement_and_controls.md`

Do not implement top-down cardinal movement.

## Feature-to-file Map

| Feature | Primary Files | Must Read | Notes |
|---|---|---|---|
| Game constants | `Source/Core/GameConstants.cs` | `docs/architecture.md` | Tile size, debug flags, default speeds |
| Input | `Source/Core/InputManager.cs` | `docs/movement_and_controls.md` | Convert keyboard state into intent |
| Quarter-view math | `Source/Core/IsoMath.cs` | `docs/iso_coordinate_system.md` | World/screen conversion |
| Camera | `Source/Core/Camera2D.cs` | `docs/iso_coordinate_system.md` | Camera is screen-space view offset |
| Tile map | `Source/World/IsoTileMap.cs` | `docs/iso_coordinate_system.md` | Draw diamond grid |
| Collision | `Source/World/CollisionMap.cs` | `docs/collision_policy.md` | World/tile collision only |
| Entity base | `Source/Entities/Entity.cs` | `docs/architecture.md` | Base update/draw object |
| Character base | `Source/Entities/Character.cs` | `docs/combat_system_design.md` | HP/stats/death |
| Player | `Source/Entities/Player.cs` | `docs/movement_and_controls.md` | Player input movement |
| Companion | `Source/Entities/Companion.cs` | `docs/companion_ai_design.md` | Follow/assist AI |
| Monster | `Source/Entities/Monster.cs` | `docs/combat_system_design.md` | Simple enemy behavior |
| Movement | `Source/Systems/IsoMovementSystem.cs` | `docs/movement_and_controls.md` | Apply world velocity |
| Companion AI | `Source/Systems/CompanionAISystem.cs` | `docs/companion_ai_design.md` | State selection |
| Monster AI | `Source/Systems/MonsterAISystem.cs` | `docs/combat_system_design.md` | Chase/attack |
| Combat | `Source/Systems/CombatSystem.cs` | `docs/combat_system_design.md` | Damage/cooldown/death |
| EXP/level | `Source/Systems/LevelSystem.cs` | `docs/leveling_design.md` | Minimal progression |
| Render sorting | `Source/Systems/RenderOrderSystem.cs` | `docs/rendering_order_policy.md` | Sort by world position |
| Speech bubble | `Source/UI/SpeechBubble.cs` | `docs/dialogue_reaction_design.md` | Text above character |
| HUD | `Source/UI/Hud.cs` | `docs/ui_policy.md` | HP/EXP debug display |
| Data objects | `Source/Data/*.cs` | Relevant design docs | Plain data only |

## Do Not Put These in Game1.cs

Do not grow `Game1.cs` into a giant file.

`Game1.cs` may own:

- GraphicsDevice setup.
- SpriteBatch.
- Loading placeholder textures/fonts.
- Creating core objects.
- Calling update/draw systems.

`Game1.cs` must not own long-term logic for:

- AI decisions.
- Combat calculations.
- EXP/level formulas.
- Collision rules.
- Dialogue reaction rules.
- Tile coordinate math.

## Recommended First Implementation Files

Phase 1:

```text
Source/Core/GameConstants.cs
Source/Core/InputManager.cs
Source/Core/Camera2D.cs
Source/Core/IsoMath.cs
```

Phase 2:

```text
Source/World/IsoTile.cs
Source/World/IsoTileMap.cs
```

Phase 3:

```text
Source/Entities/Entity.cs
Source/Entities/Character.cs
Source/Entities/Player.cs
Source/Systems/IsoMovementSystem.cs
```

## Validation Map

| Change Type | Required Command |
|---|---|
| Any C# change | `dotnet build` |
| Rendering change | `dotnet run` |
| Input/movement change | `dotnet run` |
| Docs-only change | no build required |
| Project file change | `dotnet restore`, then `dotnet build` |

## Common Mistakes to Avoid

- Using screen pixel coordinates as world coordinates.
- Drawing square top-down tiles.
- Sorting entities only by `Position.Y` without considering world transform.
- Putting all logic into `Game1.cs`.
- Adding pathfinding before simple follow works.
- Starting with final art instead of placeholder shapes.
- Adding skill trees before basic attack works.
