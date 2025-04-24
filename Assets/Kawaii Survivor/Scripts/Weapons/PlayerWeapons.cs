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
}
