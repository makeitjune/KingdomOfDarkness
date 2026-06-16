# Coding Standards

## Language

C#.

## Framework

MonoGame.

## .NET

Target expected SDK: .NET 9.

## Style

Use file-scoped namespaces:

```csharp
namespace KingdomOfDarkness.Core;
```

Use explicit classes and small methods.

Prefer readable code over clever abstractions.

## Naming

Classes:

```text
PascalCase
```

Methods:

```text
PascalCase
```

Private fields:

```csharp
private readonly SpriteBatch _spriteBatch;
```

Public properties:

```csharp
public Vector2 WorldPosition { get; set; }
```

Constants:

```csharp
public const int TileWidth = 64;
```

## Nullability

Use nullable annotations only if the project already enables them.
Do not start a large nullable migration during MVP.

## Dependencies

Do not add external NuGet packages during MVP unless explicitly required.

## Comments

Use comments to explain game-specific decisions, especially quarter-view math.

Avoid comments that repeat obvious C# syntax.

## Error Handling

For MVP, prefer simple fail-fast behavior during development.

Avoid swallowing exceptions silently.

## Game Loop

Use `gameTime.ElapsedGameTime.TotalSeconds` for frame-rate independent movement.

## Randomness

If randomness is added, keep it centralized enough for later testing.

## Overengineering Ban

Do not introduce:

- ECS.
- Service locator.
- Dependency injection container.
- Reflection-based registries.
- Scripting language.
- Plugin system.
- Generic asset database.
- Full editor tooling.

MVP first.
