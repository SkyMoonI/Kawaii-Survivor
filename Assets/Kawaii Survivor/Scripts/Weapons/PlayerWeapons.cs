using UnityEngine;
using UnityEngine.PlayerLoop;

public class PlayerWeapons : MonoBehaviour
{
    [SerializeField] private WeaponPosition[] m_weaponPositions; // Array of weapon positions

    public void AddWeapon(WeaponDataSO weaponData, int weaponLevel)
    {
        m_weaponPositions[Random.Range(0, m_weaponPositions.Length)].AssignWeapon(weaponData.Prefab, weaponLevel); // Add the weapon to a random position
    }
}
