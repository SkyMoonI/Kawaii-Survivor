using UnityEngine;
using System.Collections.Generic;
using NaughtyAttributes;
using System.Linq;

[CreateAssetMenu(fileName = "New Weapon Data", menuName = "Scriptable Objects/New Weapon Data", order = 1)]
public class WeaponDataSO : ScriptableObject
{
    [field: SerializeField] public string Name { get; private set; } // Character name
    [field: SerializeField] public Sprite Sprite { get; private set; } // Character sprite
    [field: SerializeField] public int PurchasePrice { get; private set; } // Character purchase price
    [field: SerializeField] public Weapon Prefab { get; private set; } // Character level

    [HorizontalLine]
    [SerializeField] private float m_attack;
    [SerializeField] private float m_attackSpeed;
    [SerializeField] private float m_criticalChance;
    [SerializeField] private float m_criticalDamage;
    [SerializeField] private float m_range;


    public Dictionary<Stat, StatData> BaseStats
    {
        get
        {
            return new Dictionary<Stat, StatData>
            {
                {Stat.Attack, new StatData(Stat.Attack, m_attack)},
                {Stat.AttackSpeed, new StatData(Stat.AttackSpeed, m_attackSpeed)},
                {Stat.CriticalChance, new StatData(Stat.CriticalChance, m_criticalChance)},
                {Stat.CriticalDamage, new StatData(Stat.CriticalDamage, m_criticalDamage)},
                {Stat.Range, new StatData(Stat.Range, m_range)},
            };
        }

        private set { }
    }

    public float GetStatValue(Stat stat)
    {
        if (BaseStats.TryGetValue(stat, out StatData statData))
        {
            return statData.m_value;
        }
        else
        {
            Debug.LogError($"Stat {stat} not found in BaseStats.");
            return 0f;
        }
    }
}
