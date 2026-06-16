using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace KingdomOfDarkness.Data;

/// <summary>
/// Hardcoded database of monster types for the prototype.
/// Later this could be replaced with JSON loading.
/// </summary>
public static class MonsterDatabase
{
    private static readonly Dictionary<string, MonsterData> _monsters = new();

    public static void Initialize()
    {
        // Name, MaxHP, AttackPower, Defense, MoveSpeed, AggroRange, AttackRange, ExperienceReward, DebugColor
        
        _monsters["Goblin"] = new MonsterData(
            "고블린",
            50,   // HP
            8,    // Atk
            1,    // Def
            2.5f, // Spd
            4.0f, // Aggro
            1.2f, // Range
            15,   // Exp
            Color.DarkGreen
        );

        _monsters["Skeleton"] = new MonsterData(
            "해골전사",
            70,
            12,
            3,
            2.0f,
            3.5f,
            1.2f,
            25,
            Color.LightGray
        );

        _monsters["Wolf"] = new MonsterData(
            "숲늑대",
            40,
            15,
            1,
            4.0f,
            5.0f,
            1.5f,
            20,
            Color.Brown
        );

        _monsters["DarkMage"] = new MonsterData(
            "흑마법사",
            60,
            20,
            2,
            2.0f,
            6.0f,
            4.0f,
            40,
            Color.Purple
        );
    }

    public static MonsterData GetMonsterData(string id)
    {
        if (_monsters.TryGetValue(id, out var data))
        {
            return data;
        }
        // Fallback to a default slime
        return new MonsterData("슬라임", 30, 5, 0, 1.5f, 3.0f, 1.2f, 10, Color.LightGreen);
    }
}
