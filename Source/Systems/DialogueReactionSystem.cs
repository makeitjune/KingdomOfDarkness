using System.Collections.Generic;
using Microsoft.Xna.Framework;
using KingdomOfDarkness.Entities;
using KingdomOfDarkness.UI;

namespace KingdomOfDarkness.Systems;

public enum DialogueEvent
{
    CombatStart,
    LowHealth,
    TooFar,
    KillMonster,
    Blocked
}

public class DialogueReactionSystem
{
    private readonly Dictionary<DialogueEvent, float> _cooldowns = new();
    private const float EventCooldownSeconds = 6.0f; // 6 seconds cooldown per event type

    public DialogueReactionSystem()
    {
        // Initialize cooldowns
        foreach (DialogueEvent ev in System.Enum.GetValues(typeof(DialogueEvent)))
        {
            _cooldowns[ev] = 0f;
        }
    }

    public void Update(GameTime gameTime)
    {
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
        
        // Tick down cooldowns
        var keys = new List<DialogueEvent>(_cooldowns.Keys);
        foreach (var key in keys)
        {
            if (_cooldowns[key] > 0f)
            {
                _cooldowns[key] -= dt;
                if (_cooldowns[key] < 0f)
                    _cooldowns[key] = 0f;
            }
        }
    }

    public void TriggerReaction(Companion companion, SpeechBubble bubble, DialogueEvent dialogueEvent)
    {
        if (companion == null || bubble == null || companion.IsDead) return;

        // Check if event is on cooldown
        if (_cooldowns[dialogueEvent] > 0f) return;

        string text = GetDialogueText(dialogueEvent);
        if (!string.IsNullOrEmpty(text))
        {
            bubble.Show(text, 2.5f); // Display for 2.5 seconds
            _cooldowns[dialogueEvent] = EventCooldownSeconds; // Trigger cooldown
        }
    }

    private string GetDialogueText(DialogueEvent dialogueEvent)
    {
        return dialogueEvent switch
        {
            DialogueEvent.CombatStart => "제가 도와드릴게요!",
            DialogueEvent.LowHealth   => "잠깐 뒤로 빠질게요...",
            DialogueEvent.TooFar      => "같이 가요!",
            DialogueEvent.KillMonster => "좋았어요!",
            DialogueEvent.Blocked     => "비켜주세요...",
            _                         => string.Empty
        };
    }
}
