using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace KingdomOfDarkness.Data;

public static class ClassDatabase
{
    private static readonly Dictionary<CharacterClassType, CharacterClassData> _classes = new();

    public static void Initialize()
    {
        _classes[CharacterClassType.Warrior] = new CharacterClassData
        {
            ClassType = CharacterClassType.Warrior,
            DisplayName = "전사",
            BaseHP = 150,
            BaseMP = 30,
            BaseAttack = 18,
            BaseDefense = 10,
            BaseMoveSpeed = 3.5f,
            BaseAttackRange = 1.2f,
            HpPerLevel = 50,
            MpPerLevel = 5,
            AtkPerLevel = 5,
            DefPerLevel = 3,
            DebugColor = Color.CornflowerBlue
        };

        _classes[CharacterClassType.Mage] = new CharacterClassData
        {
            ClassType = CharacterClassType.Mage,
            DisplayName = "마법사",
            BaseHP = 70,
            BaseMP = 150,
            BaseAttack = 8,
            BaseDefense = 3,
            BaseMoveSpeed = 3.5f,
            BaseAttackRange = 5.0f,
            HpPerLevel = 15,
            MpPerLevel = 40,
            AtkPerLevel = 2,
            DefPerLevel = 1,
            DebugColor = Color.MediumPurple
        };

        _classes[CharacterClassType.Priest] = new CharacterClassData
        {
            ClassType = CharacterClassType.Priest,
            DisplayName = "성직자",
            BaseHP = 90,
            BaseMP = 120,
            BaseAttack = 6,
            BaseDefense = 5,
            BaseMoveSpeed = 3.5f,
            BaseAttackRange = 1.2f,
            HpPerLevel = 25,
            MpPerLevel = 30,
            AtkPerLevel = 1,
            DefPerLevel = 2,
            DebugColor = Color.LightGoldenrodYellow
        };

        _classes[CharacterClassType.Rogue] = new CharacterClassData
        {
            ClassType = CharacterClassType.Rogue,
            DisplayName = "도적",
            BaseHP = 80,
            BaseMP = 50,
            BaseAttack = 20,
            BaseDefense = 4,
            BaseMoveSpeed = 3.5f,
            BaseAttackRange = 1.2f,
            HpPerLevel = 20,
            MpPerLevel = 10,
            AtkPerLevel = 6,
            DefPerLevel = 1,
            DebugColor = Color.DarkSlateGray
        };

        _classes[CharacterClassType.Monk] = new CharacterClassData
        {
            ClassType = CharacterClassType.Monk,
            DisplayName = "무도가",
            BaseHP = 110,
            BaseMP = 60,
            BaseAttack = 14,
            BaseDefense = 7,
            BaseMoveSpeed = 3.5f,
            BaseAttackRange = 1.2f,
            HpPerLevel = 35,
            MpPerLevel = 15,
            AtkPerLevel = 4,
            DefPerLevel = 2,
            DebugColor = Color.SaddleBrown
        };
    }

    public static CharacterClassData GetClass(CharacterClassType type)
    {
        return _classes.TryGetValue(type, out var data) ? data : _classes[CharacterClassType.Warrior];
    }
}
