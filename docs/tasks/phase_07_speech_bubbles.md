# Phase 7 Task — Speech Bubbles

## Goal

Add simple companion speech-bubble reactions.

## Must Read

- `docs/dialogue_reaction_design.md`
- `docs/ui_policy.md`

## Create

```text
Source/UI/SpeechBubble.cs
Source/Systems/DialogueReactionSystem.cs
```

## Required Behavior

- Companion can display speech above its head.
- Speech has timeout.
- Speech has cooldown.
- Speech reacts to basic events.

## First Reactions

```text
CombatStart
LowHealth
TooFar
KillMonster
Blocked
```

## Do Not

- Do not add LLM/chatbot.
- Do not add dialogue trees.
- Do not add quest dialogue.
- Do not make speech spam every frame.

## Validation

```powershell
dotnet build
dotnet run
```

## Manual Check

- Speech appears above companion.
- Speech disappears.
- Speech does not spam.
