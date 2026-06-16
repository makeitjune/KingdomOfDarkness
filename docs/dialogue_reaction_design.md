# Dialogue Reaction Design

## Goal

Make the companion feel alive using simple rule-based speech bubbles.

Do not add chatbot or LLM integration during MVP.

## SpeechBubble

Minimum fields:

```csharp
public string Text { get; set; }
public float TimeRemaining { get; set; }
public Vector2 WorldAnchor { get; set; }
```

Draw above character using world-to-screen conversion plus vertical offset.

## DialogueReactionSystem

Responsibilities:

- Receive events or inspect game state.
- Choose a line.
- Apply cooldown.
- Show speech bubble.

## First Reaction Lines

```text
CombatStart:  "제가 도와드릴게요!"
LowHealth:    "잠깐 뒤로 빠질게요..."
TooFar:       "같이 가요!"
KillMonster:  "좋았어요!"
Blocked:      "비켜주세요..."
IdleRare:     "어디로 갈까요?"
```

## Cooldown Policy

Avoid spam.

Suggested:

```text
Same reaction cooldown: 5 seconds
Any speech cooldown: 1.5 seconds
```

## Priority

Higher priority should override lower priority:

```text
LowHealth > Blocked > CombatStart > KillMonster > TooFar > IdleRare
```

## MVP Event Sources

Can be simple method calls first:

```csharp
dialogueReactionSystem.Request(companion, ReactionType.CombatStart);
```

Do not build a complex event bus yet.

## Manual Test Checklist

- Speech appears over companion.
- Speech disappears after a short time.
- Speech does not spam every frame.
- Low health reaction can override normal follow reaction.
