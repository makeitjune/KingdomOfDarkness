# Target File Structure

## Current Root

```text
KingdomOfDarkness/
├─ .config/
├─ .vscode/
├─ Content/
├─ Game1.cs
├─ Program.cs
├─ KingdomOfDarkness.csproj
├─ README.md
├─ AGENTS.md
├─ plan.md
└─ docs/
```

## Target Source Structure

```text
Source/
├─ Core/
│  ├─ GameConstants.cs
│  ├─ InputManager.cs
│  ├─ Camera2D.cs
│  └─ IsoMath.cs
│
├─ World/
│  ├─ IsoTile.cs
│  ├─ IsoTileMap.cs
│  ├─ CollisionMap.cs
│  └─ MapLoader.cs
│
├─ Entities/
│  ├─ Entity.cs
│  ├─ Character.cs
│  ├─ Player.cs
│  ├─ Companion.cs
│  └─ Monster.cs
│
├─ Systems/
│  ├─ IsoMovementSystem.cs
│  ├─ CompanionAISystem.cs
│  ├─ MonsterAISystem.cs
│  ├─ CombatSystem.cs
│  ├─ LevelSystem.cs
│  ├─ RenderOrderSystem.cs
│  └─ DialogueReactionSystem.cs
│
├─ UI/
│  ├─ Hud.cs
│  ├─ HealthBar.cs
│  ├─ Nameplate.cs
│  └─ SpeechBubble.cs
│
└─ Data/
   ├─ CharacterStats.cs
   ├─ MonsterData.cs
   ├─ SkillData.cs
   └─ ExperienceTable.cs
```

## Content Structure

```text
Content/
├─ sprites/
│  ├─ player/
│  ├─ companions/
│  ├─ monsters/
│  └─ tiles/
├─ fonts/
├─ sounds/
└─ maps/
```

## Creation Order

1. `Source/Core`
2. `Source/World`
3. `Source/Entities`
4. `Source/Systems`
5. `Source/UI`
6. `Source/Data`

Do not create empty files unless they are part of the current phase.
