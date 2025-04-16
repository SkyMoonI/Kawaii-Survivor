public enum GameState
{
    MENU,
    WEAPONSELECTION,
    GAME,
    GAMEOVER,
    STAGECOMPLETE,
    WAVETRANSITION,
    SHOP,
}

public enum Stat
{
    Attack,
    AttackSpeed,
    CriticalChance,
    CriticalDamage,
    MoveSpeed,
    MaxHealth,
    Range,
    HealthRecoverySpeed,
    Armor,
    Luck,
    Dodge,
    LifeSteal
}

public static class Enums
{
    public static string FormatStatName(Stat stat)
    {
        string unformatted = stat.ToString(); // Get the string representation of the enum value
        string formatted = stat.ToString().Substring(0, 1); // Get the first letter of the string

        for (int i = 1; i < unformatted.Length; i++)
        {
            if (char.IsUpper(unformatted[i]))
            {
                formatted += " "; // Add a space before uppercase letters
            }
            formatted += unformatted[i]; // Append the character to the formatted string
        }

        return formatted;
    }
}