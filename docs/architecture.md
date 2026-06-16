# Architecture

## Overview

KingdomOfDarkness is a simple C# + MonoGame game built around a quarter-view RPG architecture.

The architecture must support:

- Quarter-view tile rendering.
- World-coordinate gameplay logic.
- Screen-coordinate drawing.
- Player movement.
- Companion AI.
- Monster combat.
- Speech-bubble reactions.

## High-level Flow

```text
Program.cs
  └─ Game1.cs
       ├─ Initialize()
       │    └─ Create camera, map, entities, systems
       ├─ LoadContent()
       │    └─ Load or create placeholder textures/fonts
       ├─ Update()
       │    ├─ InputManager.Update()
       │    ├─ Player.Update()
       │    ├─ CompanionAISystem.Update()
       │    ├─ MonsterAISystem.Update()
       │    ├─ CombatSystem.Update()
       │    ├─ LevelSystem.Update()
       │    └─ Camera2D.Follow()
       └─ Draw()
            ├─ IsoTileMap.Draw()
            ├─ RenderOrderSystem.DrawEntities()
            └─ Hud.Draw()
```

## Main Folders

```text
Source/
├─ Core/
├─ World/
├─ Entities/
├─ Systems/
├─ UI/
└─ Data/
```

## Core Layer

The Core layer has general reusable runtime helpers.

Expected files:

```text
GameConstants.cs
InputManager.cs
Camera2D.cs
IsoMath.cs
```

### Responsibilities

- Store constants.
- Read keyboard/mouse state.
- Convert world coordinates to screen coordinates.
- Manage camera offset.

### Must Not

- Implement combat.
- Implement monster AI.
- Store level data.
- Own map data.

## World Layer

The World layer owns map and collision information.

Expected files:

```text
IsoTile.cs
IsoTileMap.cs
CollisionMap.cs
MapLoader.cs
```

### Responsibilities

- Store tile data.
- Render diamond tile map.
- Expose blocking data.
- Convert tile access into safe map queries.

### Must Not

- Own player logic.
- Apply damage.
- Control companion state.

## Entities Layer

Entities are game objects.

Expected files:

```text
Entity.cs
Character.cs
Player.cs
Companion.cs
Monster.cs
```

### Entity

Base object:

```text
Id
WorldPosition
IsActive
DrawOrder
Update()
Draw()
```

### Character

Combat-capable entity:

```text
Name
Stats
CurrentHP
IsDead
Target
AttackCooldown
```

### Player

Player-controlled character.

### Companion

AI-controlled party member.

### Monster

Enemy character.

## Systems Layer

Systems apply rules across entities.

Expected files:

```text
IsoMovementSystem.cs
CompanionAISystem.cs
MonsterAISystem.cs
CombatSystem.cs
LevelSystem.cs
RenderOrderSystem.cs
DialogueReactionSystem.cs
```

### Rule

Systems should be small and explicit.

Example:

- `CombatSystem` should calculate and apply damage.
- `CompanionAISystem` should decide companion state.
- `RenderOrderSystem` should sort drawables.

Do not make one giant `GameSystem`.

## UI Layer

UI draws non-world overlays.

Expected files:

```text
Hud.cs
HealthBar.cs
Nameplate.cs
SpeechBubble.cs
```

UI can read entity state but should not modify gameplay rules.

## Data Layer

Data classes should be simple.

Expected files:

```text
CharacterStats.cs
MonsterData.cs
SkillData.cs
ExperienceTable.cs
```

For MVP, use hardcoded defaults or small data classes.
Do not add JSON loading until the basic loop works.

## Game1.cs Policy

`Game1.cs` is an orchestrator, not a dumping ground.

Allowed:

- Object creation.
- Load content.
- Call update/draw methods.
- Debug bootstrapping.

Avoid:

- Large combat logic.
- Long AI decision logic.
- Coordinate math beyond simple calls to `IsoMath`.
- Big switch statements for all game states.

## Placeholder Rendering Policy

Early MVP can use generated textures:

- White rectangle pixel.
- Diamond tile using line drawing or simple polygon approximation.
- Colored squares/circles for characters.

Final art is not required before gameplay works.

## Future Expansion

After MVP:

```text
Inventory
Equipment
Skill system
Quest system
Town/NPC system
Save/load
More companions
Better pathfinding
Animation pipeline
```

These must not be started before the quarter-view movement/combat prototype works.
