# Prompt for Gemini 3.5 Flash Lite

Use this prompt when asking Gemini to implement the project.

```text
You are working inside my local C# + MonoGame project named KingdomOfDarkness.

Read AGENTS.md first.
Then read plan.md.
Then read docs/developer_feature_map.md.
Then read docs/iso_coordinate_system.md.
Then implement only the phase I specify.

This is not a top-down game.
It is a 2D quarter-view / isometric-style RPG inspired by old Korean RPGs like 어둠의 전설 and 아스가르드.
Movement must feel diagonal on screen.
Gameplay positions must be stored in world coordinates.
Drawing must convert world coordinates to screen coordinates using IsoMath.

Use C# + MonoGame only.
Do not use Godot.
Do not use Unity.
Do not add external packages.
Do not put everything into Game1.cs.
Keep the project compiling.

Before editing, run or inspect:
git status

After editing, run:
dotnet build

If the task changes visual behavior, also run:
dotnet run

Report:
1. Files changed
2. What was implemented
3. What command was run
4. Whether build passed
5. What should be done next

Now implement: [WRITE THE PHASE HERE]
```

## Recommended First Gemini Task

```text
Implement Phase 1 from docs/tasks/phase_01_core_helpers.md only.
Do not implement Phase 2.
Keep the game compiling.
```

## Recommended Second Gemini Task

```text
Implement Phase 2 from docs/tasks/phase_02_quarter_view_map.md only.
Render a 10x10 quarter-view diamond test map.
Use IsoMath.WorldToScreen.
Keep the game compiling.
```

## Recommended Third Gemini Task

```text
Implement Phase 3 from docs/tasks/phase_03_player_movement.md only.
Add player movement using world coordinates and quarter-view screen-diagonal movement.
Do not implement combat yet.
Keep the game compiling.
```
