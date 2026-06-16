# No Shortcuts Policy

## Purpose

This file prevents agents from accidentally building the wrong kind of game.

## Forbidden

Do not:

- Convert the design into a top-down RPG.
- Use Godot.
- Use Unity.
- Add a different engine.
- Replace MonoGame.
- Implement movement directly in screen pixels.
- Implement square top-down tile map as the main map.
- Put all logic in `Game1.cs`.
- Add networking.
- Add multiplayer.
- Add LLM/chatbot integration.
- Add full inventory before movement/combat.
- Add full quest system before movement/combat.
- Add 4 companions before 1 companion works.
- Add complex pathfinding before simple following works.
- Require final art assets before placeholder gameplay works.

## Required

Always:

- Keep quarter-view coordinate conversion.
- Store gameplay positions in world coordinates.
- Convert to screen coordinates during draw.
- Keep the project compiling.
- Implement one small slice at a time.
- Prefer visible gameplay progress over theoretical architecture.
