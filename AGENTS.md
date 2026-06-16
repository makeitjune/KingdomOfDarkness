# AGENTS.md — KingdomOfDarkness

## Role

You are an implementation agent working on **KingdomOfDarkness**, a C# + MonoGame 2D quarter-view RPG prototype.

Your job is to make small, safe, verifiable changes that move the project toward the current MVP:
a **Dark Ages / Asgard-style quarter-view RPG** with diagonal screen movement, companion AI, simple monsters, combat, leveling, and speech-bubble reactions.

## Non-negotiable Game Direction

This is **NOT** a top-down game.

Use this direction:

- Visual style: 2D quarter-view / isometric-like RPG.
- Reference feeling: **어둠의 전설 / 아스가르드 style**.
- Movement feeling: screen-diagonal movement.
- World logic: grid/tile/world coordinates.
- Rendering: convert world coordinates to screen coordinates.
- Depth sorting: entities farther “down” the screen draw later.
- Engine/framework: **C# + MonoGame**, not Godot, not Unity.

Whenever a document or task says “top-down”, treat that as wrong and correct it to **quarter-view / isometric-style**.

## Project Environment

Expected current project:

```text
KingdomOfDarkness/
├─ .config/
├─ .vscode/
├─ Content/
├─ Game1.cs
├─ Program.cs
├─ KingdomOfDarkness.csproj
├─ README.md
└─ AGENTS.md
```

Expected tools:

```powershell
dotnet --version
# expected: 9.x

dotnet build
dotnet run
```

MonoGame template installed:

```powershell
dotnet new install MonoGame.Templates.CSharp
```

## Required Reading Order

Before changing code, read:

1. `plan.md`
2. `docs/developer_feature_map.md`
3. `docs/architecture.md`
4. `docs/iso_coordinate_system.md`
5. Relevant task file under `docs/tasks/`

If a task is unclear, first check `docs/developer_feature_map.md`.

## Core Design Rule

Keep the code simple and direct.

Use:

- Simple OOP classes.
- Clear namespaces.
- Small systems.
- Small files.
- Explicit update/draw flow.
- No ECS for now.
- No dependency injection framework.
- No external packages unless the plan explicitly asks for it.
- No editor-specific assumptions.
- No Godot/Unity-style scene architecture.

## Current MVP Target

`KingdomOfDarkness 0.1` must prove:

1. Quarter-view coordinate conversion works.
2. A diamond/isometric test map renders.
3. Player moves diagonally on screen using keyboard.
4. Camera follows player.
5. Entity rendering order works.
6. Companion follows player with spacing.
7. One monster can be attacked.
8. Combat has HP, damage, cooldown, death.
9. Monster kill grants EXP.
10. Companion can show simple speech bubbles.

## Implementation Style

Prefer incremental changes.

A good change:

- Compiles.
- Has no unrelated refactor.
- Adds one clear feature.
- Keeps the project runnable with `dotnet run`.
- Updates docs/checklist if behavior changes.

A bad change:

- Rewrites the whole project.
- Introduces Godot/Unity concepts.
- Implements generic engine architecture before gameplay.
- Adds networking/multiplayer too early.
- Adds complex asset pipelines before placeholder rendering works.
- Changes movement to top-down cardinal movement.

## Folder Policy

Create these source folders as implementation grows:

```text
Source/
├─ Core/
├─ World/
├─ Entities/
├─ Systems/
├─ UI/
└─ Data/
```

Use namespace:

```csharp
namespace KingdomOfDarkness.Core;
namespace KingdomOfDarkness.World;
namespace KingdomOfDarkness.Entities;
namespace KingdomOfDarkness.Systems;
namespace KingdomOfDarkness.UI;
namespace KingdomOfDarkness.Data;
```

## Coordinate Policy

Always separate:

```text
World position = logical game position
Screen position = pixel position after quarter-view transform
```

Do not store gameplay logic directly in screen pixel coordinates.

Default tile size:

```text
TileWidth = 64
TileHeight = 32
```

Default transform:

```csharp
screenX = (worldX - worldY) * TileWidth / 2f;
screenY = (worldX + worldY) * TileHeight / 2f;
```

Read `docs/iso_coordinate_system.md` before changing movement, tile map, camera, collision, AI, or rendering order.

## Validation

After every implementation step:

```powershell
dotnet build
```

When visual/gameplay behavior is changed:

```powershell
dotnet run
```

If you cannot run the game, still run `dotnet build` and clearly report what was not manually verified.

## Git Policy

Before coding:

```powershell
git status
```

Do not overwrite user changes.

Make focused commits if asked by the user. Suggested commit message format:

```text
Add quarter-view coordinate helpers
Add isometric test map rendering
Add player diagonal movement
Add companion follow prototype
Add simple combat prototype
```

## Communication Policy for AI Agents

When reporting back, include:

- What files changed.
- What behavior changed.
- What command was run.
- Whether build passed.
- What remains.

Do not claim visual behavior is correct unless `dotnet run` was actually checked.

## Forbidden Shortcuts

Do not:

- Switch to Godot, Unity, or another engine.
- Rename the game unless asked.
- Replace the MonoGame template with a different framework.
- Implement top-down movement.
- Put all logic into `Game1.cs`.
- Use screen-space collision for gameplay.
- Add networking, cloud saves, multiplayer, or procedural generation during MVP.
- Add chatbot/LLM integration during MVP.
- Add 4 companions before 1 companion works.
- Add a full quest system before core movement/combat works.

## When in Doubt

Choose the simplest runnable implementation that preserves the quarter-view RPG direction.

If a feature is too large, implement only the smallest visible slice and update the checklist.
