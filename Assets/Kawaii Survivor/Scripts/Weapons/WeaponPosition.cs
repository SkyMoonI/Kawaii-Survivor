using UnityEngine;

public class WeaponPosition : MonoBehaviour
{
    [Header("Elements")]
    public Weapon Weapon { get; private set; } // Reference to the weapon prefab
    public void AssignWeapon(Weapon weaponPrefab, int weaponLevel)
    {
        Weapon = Instantiate(weaponPrefab, transform); // Instantiate the weapon prefab at the position of this object

        Weapon.transform.localPosition = Vector3.zero; // Set the local position to zero (centered in the parent)
        Weapon.transform.localRotation = Quaternion.identity; // Set the local rotation to identity (no rotation)

        Weapon.UpgradeTo(weaponLevel); // Upgrade the weapon to the specified level
    }
}
