# ADR 0003 — MVP Before Full RPG Systems

## Status

Accepted.

## Context

The long-term vision includes companions, skills, quests, equipment, towns, personality, and chatbot-like reactions.

Implementing all at once would make the project too complex.

## Decision

Build the smallest playable MVP first:

```text
quarter-view movement
+ one companion
+ one monster
+ simple combat
+ EXP
+ speech bubble
```

## Consequences

Do not implement full systems until MVP works.

Deferred:

- Inventory.
- Equipment.
- Skills.
- Quests.
- Save/load.
- Town NPCs.
- Multiple companions.
- Chatbot integration.
