using Microsoft.Xna.Framework;

namespace KingdomOfDarkness.Data;

public enum CharacterClassType
{
    Warrior,
    Mage,
    Priest,
    Rogue,
    Monk
}

public class CharacterClassData
{
    public CharacterClassType ClassType { get; set; }
    public string DisplayName { get; set; }
    
    // Base Stats
    public int BaseHP { get; set; }
    public int BaseMP { get; set; }
    public int BaseAttack { get; set; }
    public int BaseDefense { get; set; }
    
    // Action Params
    public float BaseMoveSpeed { get; set; }
    public float BaseAttackRange { get; set; }
    
    // Level Up Growth
    public int HpPerLevel { get; set; }
    public int MpPerLevel { get; set; }
    public int AtkPerLevel { get; set; }
    public int DefPerLevel { get; set; }

    public Color DebugColor { get; set; }
}
