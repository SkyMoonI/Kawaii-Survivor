using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class PlayerWeapons : MonoBehaviour
{
    [SerializeField] private WeaponPosition[] m_weaponPositions; // Array of weapon positions

    public bool TryAddWeapon(WeaponDataSO weaponData, int weaponLevel) // Method to try to add a weapon to the player's inventory
    {
        for (int i = 0; i < m_weaponPositions.Length; i++) // Iterate through the weapon positions
        {
            if (m_weaponPositions[i].Weapon == null) // If the weapon position is empty
            {
                m_weaponPositions[i].AssignWeapon(weaponData.Prefab, weaponLevel); // Add the weapon to the position
                return true;
            }
        }

        return false;
    }

    public Weapon[] GetWeapons()
    {
        List<Weapon> weapons = new List<Weapon>(); // List to hold the weapons

        foreach (WeaponPosition weaponPosition in m_weaponPositions) // Iterate through the weapon positions
        {
            if (weaponPosition.Weapon != null) // If the weapon position has a weapon
            {
                weapons.Add(weaponPosition.Weapon); // Add the weapon to the list
            }
            else
            {
                weapons.Add(null); // Add null to the list so that the list is always the same length (6)
            }
        }

        return weapons.ToArray(); // Return the array of weapons
    }

    public void RecycleWeapon(int weaponIndex)
    {
        for (int i = 0; i < m_weaponPositions.Length; i++) // Iterate through the weapon positions
        {
            if (weaponIndex != i)
            {
                continue;
            }
            else
            {
                int recyclePrice = WeaponStatsCalculator.GetRecyclePrice(m_weaponPositions[i].Weapon.WeaponData, m_weaponPositions[i].Weapon.Level); // Get the recycle price of the weapon
                CurrencyManager.Instance.AddCurrency(recyclePrice); // Add the recycle price to the player's currency

                m_weaponPositions[i].RemoveWeapon(); // Recycle the weapon.

                return;
            }
        }
    }
}
