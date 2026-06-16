using KingdomOfDarkness.Entities;

namespace KingdomOfDarkness.Systems;

public class LevelSystem
{
    /// <summary>
    /// Checks if a character is eligible for a level up and applies upgrades.
    /// Returns true if the character leveled up.
    /// </summary>
    public bool CheckLevelUp(Character character)
    {
        if (character == null || character.IsDead) return false;

        bool leveledUp = false;
        
        // Simple formula: 100 XP per level to level up
        while (character.Experience >= GetRequiredExperience(character.Level))
        {
            character.Experience -= GetRequiredExperience(character.Level);
            character.Level++;
            leveledUp = true;

            // Stat growth
            character.MaxHP += 15;
            character.CurrentHP = character.MaxHP; // Heal to full on level up
            character.AttackPower += 3;
            character.Defense += 1;
        }

        return leveledUp;
    }

    public int GetRequiredExperience(int currentLevel)
    {
        return currentLevel * 100;
    }
}
