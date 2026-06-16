# Implementation Checklist

## Phase 0

- [ ] `.gitignore` exists.
- [ ] `bin/` ignored.
- [ ] `obj/` ignored.
- [ ] `AGENTS.md` exists.
- [ ] `plan.md` exists.
- [ ] `docs/` exists.
- [ ] `dotnet build` passes.

## Phase 1

- [ ] `Source/Core/GameConstants.cs`
- [ ] `Source/Core/IsoMath.cs`
- [ ] `Source/Core/InputManager.cs`
- [ ] `Source/Core/Camera2D.cs`
- [ ] `IsoMath.WorldToScreen()`
- [ ] `IsoMath.ScreenToWorldApprox()`
- [ ] `dotnet build` passes.

## Phase 2

- [ ] `Source/World/IsoTile.cs`
- [ ] `Source/World/IsoTileMap.cs`
- [ ] 10x10 diamond map renders.
- [ ] Tiles use quarter-view transform.
- [ ] `dotnet build` passes.
- [ ] `dotnet run` visually checked.

## Phase 3

- [ ] `Source/Entities/Entity.cs`
- [ ] `Source/Entities/Character.cs`
- [ ] `Source/Entities/Player.cs`
- [ ] `Source/Systems/IsoMovementSystem.cs`
- [ ] Player stores world position.
- [ ] Player moves diagonally on screen.
- [ ] `dotnet build` passes.
- [ ] `dotnet run` visually checked.

## Phase 4

- [ ] `Source/Core/Camera2D.cs` follow behavior works.
- [ ] `Source/Systems/RenderOrderSystem.cs`
- [ ] Entities sorted by quarter-view depth.
- [ ] `dotnet build` passes.

## Phase 5

- [ ] `Source/Entities/Companion.cs`
- [ ] `Source/Systems/CompanionAISystem.cs`
- [ ] Companion follows player.
- [ ] Companion keeps distance.
- [ ] Companion does not constantly overlap.
- [ ] `dotnet build` passes.

## Phase 6

- [ ] `Source/Entities/Monster.cs`
- [ ] `Source/Systems/CombatSystem.cs`
- [ ] `Source/Systems/MonsterAISystem.cs`
- [ ] HP/damage/cooldown works.
- [ ] Monster can die.
- [ ] EXP granted once.
- [ ] Companion assists.
- [ ] `dotnet build` passes.

## Phase 7

- [ ] `Source/UI/SpeechBubble.cs`
- [ ] `Source/Systems/DialogueReactionSystem.cs`
- [ ] Speech appears.
- [ ] Speech timeout works.
- [ ] Speech cooldown works.
- [ ] `dotnet build` passes.

## Phase 8

- [ ] `Source/World/CollisionMap.cs`
- [ ] Blocked tiles work.
- [ ] Collision uses world/tile coordinates.
- [ ] `dotnet build` passes.
