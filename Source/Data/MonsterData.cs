using Microsoft.Xna.Framework;

namespace KingdomOfDarkness.Data;

/// <summary>
/// Immutable definition for a type of monster.
/// </summary>
public record MonsterData(
    string Name,
    int MaxHP,
    int AttackPower,
    int Defense,
    float MoveSpeed,
    float AggroRange,
    float AttackRange,
    int ExperienceReward,
    Color DebugColor
);
