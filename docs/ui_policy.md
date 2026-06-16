# UI Policy

## MVP UI

Keep UI minimal.

Required MVP UI:

- Player HP.
- Companion HP.
- Monster HP if targeted/nearby.
- EXP/Level debug line.
- Speech bubble above companion.

## Draw Layers

UI draws after world rendering.

Do not include UI elements in world depth sorting.

## Fonts

If no font is configured yet:

- Add a SpriteFont through MonoGame Content Pipeline later.
- For earlier phases, use window title/debug logs if necessary.

## Health Bars

Health bars may be world-space overlays above characters.

This means they can use `IsoMath.WorldToScreen()` plus offset.

HUD is screen-space.

## Do Not Build Yet

Do not build:

- Full inventory UI.
- Skill tree UI.
- Quest journal UI.
- Dialogue choice UI.
- Settings menu.

These are after MVP.
