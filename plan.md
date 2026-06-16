# KingdomOfDarkness — Implementation Plan

## One-line Goal

Build a **C# + MonoGame quarter-view RPG prototype** inspired by the feel of old Korean 2D RPGs like **어둠의 전설 / 아스가르드**, focused on companion AI party hunting.

## Current Technical Base

- Language: C#
- Framework: MonoGame 3.8.4.1
- SDK: .NET 9
- IDE: VS Code
- Project type: `mgdesktopgl`
- Initial project file: `KingdomOfDarkness.csproj`

## MVP Name

```text
KingdomOfDarkness 0.1 — Quarter-view Companion Combat Prototype
```

## MVP Feature Definition

The MVP is complete when the player can:

1. Run the game.
2. See a quarter-view diamond tile test map.
3. Move a player character diagonally on screen.
4. Have the camera follow the player.
5. See one companion follow the player while keeping distance.
6. Encounter one monster.
7. Attack the monster.
8. Have the companion help attack.
9. Kill the monster and gain experience.
10. See simple companion speech-bubble reactions.

## Non-goals for MVP

Do not implement these yet:

- Full RPG content.
- Full inventory.
- Equipment.
- Skill tree.
- Quest system.
- Town NPC system.
- Chatbot/LLM conversation.
- Save/load.
- Multiplayer.
- Steam integration.
- Advanced animations.
- Full art pipeline.
- Complex pathfinding.
- Procedural maps.

## Phase 0 — Project Hygiene

### Goal

Make the project safe for AI-assisted implementation.

### Tasks

- Add `.gitignore` if missing.
- Ensure `bin/` and `obj/` are ignored.
- Add `AGENTS.md`.
- Add `docs/` planning files.
- Confirm `dotnet build` passes.

### Acceptance Criteria

```powershell
dotnet build
```

passes.

## Phase 1 — Source Structure and Core Helpers

### Goal

Move away from one-file `Game1.cs` growth and create stable source folders.

### Create

```text
Source/
├─ Core/
│  ├─ GameConstants.cs
│  ├─ InputManager.cs
│  ├─ Camera2D.cs
│  └─ IsoMath.cs
├─ World/
├─ Entities/
├─ Systems/
├─ UI/
└─ Data/
```

### Required classes

- `GameConstants`
- `InputManager`
- `Camera2D`
- `IsoMath`

### Acceptance Criteria

- Project compiles.
- `Game1.cs` uses `GameConstants` for screen/tile constants.
- `IsoMath.WorldToScreen()` exists.
- `IsoMath.ScreenToWorldApprox()` exists or is explicitly deferred.

## Phase 2 — Quarter-view Test Map

### Goal

Render a visible diamond tile map without external art.

### Implement

- `IsoTile`
- `IsoTileMap`
- Placeholder diamond drawing.
- World-to-screen rendering.
- Camera offset support.

### Acceptance Criteria

- A 10x10 or larger diamond tile map appears.
- Tiles are arranged in quarter-view/isometric formation.
- No top-down square grid rendering.
- `dotnet build` passes.
- `dotnet run` shows visible map.

## Phase 3 — Player Diagonal Movement

### Goal

Player moves with quarter-view screen-diagonal feeling.

### Implement

- `Entity`
- `Character`
- `Player`
- Input mapping.
- World-position movement.
- Screen-position rendering via `IsoMath`.

### Input rule

Recommended first mapping:

```text
W = worldY - 1
S = worldY + 1
A = worldX - 1
D = worldX + 1
```

After `WorldToScreen`, this creates diagonal screen movement.

### Acceptance Criteria

- Player moves diagonally on screen.
- Movement is frame-rate independent.
- Player position is stored in world coordinates.
- Player is not implemented as top-down pixel movement.
- `dotnet build` passes.

## Phase 4 — Camera Follow

### Goal

Camera follows player in screen space while world logic stays in world space.

### Implement

- `Camera2D.Position`
- `Camera2D.GetTransform()` or simple offset method.
- Player remains roughly centered.

### Acceptance Criteria

- Moving player scrolls the map/camera.
- Camera does not modify world positions.
- `dotnet build` passes.

## Phase 5 — Render Ordering

### Goal

Entities draw in correct front/back order for quarter-view.

### Implement

- `RenderOrderSystem`
- Entity draw sort key based on world position.

### Suggested first sort key

```text
sortKey = worldX + worldY
```

Add `DrawLayer` later if needed.

### Acceptance Criteria

- Entities lower on the screen draw over entities behind them.
- Tile rendering and entity rendering are separate.
- `dotnet build` passes.

## Phase 6 — Companion Follow Prototype

### Goal

Add one companion that follows the player naturally.

### Implement

- `Companion`
- `CompanionAISystem`
- Follow distance bands.
- Basic idle/follow states.

### Suggested distances

```text
Too close:   < 0.6 world units
Comfortable: 0.6 to 1.8 world units
Too far:     > 1.8 world units
```

### Acceptance Criteria

- Companion follows player.
- Companion does not overlap player constantly.
- Companion stops near the player.
- Companion position uses world coordinates.
- `dotnet build` passes.

## Phase 7 — Monster and Combat

### Goal

Add one monster and simple combat.

### Implement

- `Monster`
- `CombatSystem`
- HP, MaxHP
- AttackPower
- AttackRange
- AttackCooldown
- Death flag
- Target selection

### First combat rule

```text
If attacker is alive,
and target is alive,
and distance <= attack range,
and cooldown is ready,
then deal damage.
```

### Acceptance Criteria

- Monster can be damaged.
- Monster can die.
- Player can be damaged.
- Companion can help attack.
- No full skill system yet.
- `dotnet build` passes.

## Phase 8 — EXP and Level

### Goal

Add minimal RPG growth loop.

### Implement

- `CharacterStats`
- `LevelSystem`
- EXP reward on monster death.
- Level up when EXP threshold reached.

### Acceptance Criteria

- Killing monster grants EXP.
- Level increases after enough EXP.
- MaxHP or AttackPower increases on level up.
- `dotnet build` passes.

## Phase 9 — Speech Bubble Reactions

### Goal

Make the companion feel alive with rule-based reactions.

### Implement

- `SpeechBubble`
- `DialogueReactionSystem`
- Reaction cooldown.
- Text display over companion.

### First reactions

```text
CombatStart: "제가 도와드릴게요!"
LowHealth:   "잠깐 뒤로 빠질게요..."
TooFar:      "같이 가요!"
KillMonster: "좋았어요!"
Blocked:     "비켜주세요..."
```

### Acceptance Criteria

- Speech bubble appears above companion.
- Reactions are rule-based.
- No LLM/chatbot integration yet.
- `dotnet build` passes.

## Phase 10 — Collision and Simple Map Blocking

### Goal

Add minimal collision after movement/combat proves fun.

### Implement

- `CollisionMap`
- Blocking tile flag.
- Prevent movement into blocked tiles.
- Optional debug overlay.

### Acceptance Criteria

- Player cannot walk through blocked tiles.
- Companion tries to follow without constant collision bugs.
- Blocking uses world/tile coordinates, not raw screen pixels.
- `dotnet build` passes.

## Recommended Implementation Order for Gemini

Give Gemini one phase at a time.

Best first prompt:

```text
Read AGENTS.md and plan.md. Implement Phase 1 only. Do not start Phase 2. Keep the game compiling. Use C# + MonoGame. Do not implement top-down movement.
```

After each phase, verify with:

```powershell
dotnet build
dotnet run
```

## Quality Bar

The MVP does not need final art.

It must feel structurally correct:

- Quarter-view coordinate system is correct.
- Player movement is diagonal on screen.
- Companion uses world logic, not fake screen following.
- Systems are separated enough to grow.
- Code remains easy to read.
