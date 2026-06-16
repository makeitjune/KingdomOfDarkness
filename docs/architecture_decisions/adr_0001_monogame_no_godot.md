# ADR 0001 — Use C# + MonoGame, Not Godot

## Status

Accepted.

## Context

The project started from the idea of making a 2D RPG. Godot was considered, but the user decided the custom RPG/AI companion behavior may be clearer to build directly.

## Decision

Use:

```text
C# + MonoGame
```

Do not use:

```text
Godot
Unity
Custom C++ engine
```

for the current project.

## Consequences

Positive:

- Direct control over architecture.
- C# is comfortable for the developer.
- Good fit for custom systems and AI logic.
- Less engine editor complexity.

Negative:

- More systems must be built manually.
- No built-in scene editor workflow.
- More responsibility for rendering/collision/tooling.

## Notes

This decision can be revisited after MVP, but implementation agents should not switch frameworks.
