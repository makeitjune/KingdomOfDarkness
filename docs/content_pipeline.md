# MonoGame Content Pipeline

## Purpose

This project uses MonoGame. Assets usually go through `Content.mgcb`.

## MVP Asset Policy

Use placeholders first.

Acceptable early placeholders:

- Generated white 1x1 texture.
- Generated diamond tile texture.
- Colored rectangles for player/companion/monster.
- No sound.
- No final sprites.

## Folder Convention

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

## What to Commit

Commit:

- `Content/Content.mgcb`
- Source images.
- SpriteFont definitions.
- Small test assets.

Do not commit:

- `bin/`
- `obj/`
- generated build outputs.

## Recommended Order

1. Placeholder shapes in code.
2. Add simple tile image.
3. Add placeholder character sprites.
4. Add SpriteFont for speech/HUD.
5. Add animation frames later.

## Do Not Block Gameplay on Art

If art is missing, create generated textures in code.
Gameplay structure comes first.
