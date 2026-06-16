using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using KingdomOfDarkness.UI;
using KingdomOfDarkness.Data;

namespace KingdomOfDarkness.Entities;

public enum CompanionState
{
    Idle,
    FollowPlayer,
    AssistAttack,
    Retreat,
    Dead
}

public class Companion : Character
{
    public bool IsRecruited { get; set; } = false;
    public CompanionState State { get; set; } = CompanionState.Idle;
    public SpeechBubble Bubble { get; } = new SpeechBubble();
    public float RespawnCooldownRemaining { get; set; } = 0f;

    public Companion(Texture2D whitePixel, Vector2 startWorldPosition, string name, CharacterClassType classType)
        : base(
            whitePixel,
            name,
            ClassDatabase.GetClass(classType)
        )
    {
        WorldPosition = startWorldPosition;
    }
}
