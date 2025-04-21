using System.Collections.Generic;
using UnityEngine;

public static class WeaponStatsCalculator
{
    public static Dictionary<Stat, StatData> GetStats(WeaponDataSO weaponData, int level)
    {
        float levelMultiplier = 1 + (level / 3f); // Calculate the level multiplier based on the weapon level

        Dictionary<Stat, StatData> calculatedStats = new Dictionary<Stat, StatData>(); // Create a dictionary to hold the stats

        foreach (KeyValuePair<Stat, StatData> stat in weaponData.BaseStats) // Iterate through the base stats of the weapon data
        {
            if (weaponData.Prefab.GetType() == typeof(MeleeWeapon) && stat.Key == Stat.Range) // Check if the weapon is a melee weapon and the stat is range
            {
                calculatedStats.Add(stat.Key, new StatData(stat.Key, stat.Value.value)); // Add the stat to the calculated stats dictionary with the level multiplier applied
            }
            else
            {
                calculatedStats.Add(stat.Key, new StatData(stat.Key, stat.Value.value * levelMultiplier)); // Add the stat to the calculated stats dictionary with the level multiplier applied
            }
        }

        return calculatedStats;
    }
}
