using System;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMerger : MonoBehaviour
{
    public static WeaponMerger Instance { get; private set; }

    [Header("Elements")]
    [SerializeField] private PlayerWeapons m_playerWeapons;

    [Header("Settings")]
    private List<Weapon> m_weaponsToMerge = new List<Weapon>();

    [Header("Actions")]
    public static Action<Weapon> onMerge;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this; // Set the singleton instance to this object
        }
        else
        {
            Destroy(gameObject); // Destroy this object if another instance already exists
        }
    }

    public bool CanMerge(Weapon weapon)
    {
        if (weapon.Level >= 3)
        {
            return false;
        }

        m_weaponsToMerge.Clear();
        m_weaponsToMerge.Add(weapon);

        Weapon[] weapons = m_playerWeapons.GetWeapons();

        foreach (Weapon playerWeapon in weapons)
        {
            // we can't merge with null weapon, a weapon with itself, a weapon with a different name(kine), or a weapon with a different level
            if (playerWeapon == null || playerWeapon == weapon || playerWeapon.WeaponData.Name != weapon.WeaponData.Name || playerWeapon.Level != weapon.Level)
            {
                continue;
            }
            else
            {
                m_weaponsToMerge.Add(playerWeapon);
                return true;
            }
        }
        return false;
    }

    public void Merge()
    {
        if (m_weaponsToMerge.Count < 2)
        {
            Debug.LogError("Not enough weapons to merge.");
            return;
        }

        DestroyImmediate(m_weaponsToMerge[1].gameObject);

        m_weaponsToMerge[0].UpgradeTo(m_weaponsToMerge[0].Level + 1);

        Weapon weapon = m_weaponsToMerge[0];

        m_weaponsToMerge.Clear();


        onMerge?.Invoke(weapon);
    }
}
