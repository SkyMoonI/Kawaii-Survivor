using System.Collections.Generic;
using UnityEngine;

public static class WeaponStatsCalculator
{
    public static Dictionary<Stat, float> GetStats(WeaponDataSO weaponData, int level)
    {
        float levelMultiplier = 1 + (level / 3f); // Calculate the level multiplier based on the weapon level

        Dictionary<Stat, float> calculatedStats = new Dictionary<Stat, float>(); // Create a dictionary to hold the stats

        foreach (KeyValuePair<Stat, StatData> stat in weaponData.BaseStats) // Iterate through the base stats of the weapon data
        {
            if (weaponData.Prefab.GetType() == typeof(MeleeWeapon) && stat.Key == Stat.Range) // Check if the weapon is a melee weapon and the stat is range
            {
                calculatedStats.Add(stat.Key, stat.Value.value); // Add the stat to the calculated stats dictionary with the level multiplier applied
            }
            else
            {
                calculatedStats.Add(stat.Key, stat.Value.value * levelMultiplier); // Add the stat to the calculated stats dictionary with the level multiplier applied
            }
        }

        return calculatedStats;
    }

    public static int GetPurchasePrice(WeaponDataSO weaponData, int level)
    {
        float multiplier = 1 + (level / 3f); // Calculate the level multiplier based on the weapon level

        return (int)(weaponData.PurchasePrice * multiplier); // Calculate the purchase price based on the weapon data and level
    }
}
