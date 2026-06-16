using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using KingdomOfDarkness.UI;

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
    public CompanionState State { get; set; } = CompanionState.Idle;
    public SpeechBubble Bubble { get; } = new SpeechBubble();

    public Companion(Texture2D whitePixel, Vector2 startWorldPosition)
        : base(
            whitePixel,
            "Companion",
            80,  // MaxHP
            12,  // AttackPower
            3,   // Defense
            3.2f // MoveSpeed (slightly slower than player)
        )
    {
        WorldPosition = startWorldPosition;
        DebugColor = Color.MediumPurple; // Distinct purple placeholder
    }
}
