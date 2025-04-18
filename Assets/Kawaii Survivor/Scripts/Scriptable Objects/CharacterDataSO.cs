using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character Data", menuName = "Scriptable Objects/New Character Data", order = 1)]
public class CharacterDataSO : ScriptableObject
{
    [field: SerializeField] public string Name { get; private set; } // Character name
    [field: SerializeField] public Sprite Sprite { get; private set; } // Character sprite
    [field: SerializeField] public int PurchasePrice { get; private set; } // Character purchase price

    [HorizontalLine]
    [SerializeField] private float m_attack;
    [SerializeField] private float m_attackSpeed;
    [SerializeField] private float m_criticalChance;
    [SerializeField] private float m_criticalDamage;
    [SerializeField] private float m_moveSpeed;
    [SerializeField] private float m_maxHealth;
    [SerializeField] private float m_range;
    [SerializeField] private float m_healthRecoverySpeed;
    [SerializeField] private float m_armor;
    [SerializeField] private float m_luck;
    [SerializeField] private float m_dodge;
    [SerializeField] private float m_lifeSteal;

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
                {Stat.MoveSpeed, new StatData(Stat.MoveSpeed, m_moveSpeed)},
                {Stat.MaxHealth, new StatData(Stat.MaxHealth, m_maxHealth)},
                {Stat.Range, new StatData(Stat.Range, m_range)},
                {Stat.HealthRegenation, new StatData(Stat.HealthRegenation, m_healthRecoverySpeed)},
                {Stat.Armor, new StatData(Stat.Armor, m_armor)},
                {Stat.Luck, new StatData(Stat.Luck, m_luck)},
                {Stat.Dodge, new StatData(Stat.Dodge, m_dodge)},
                {Stat.LifeSteal, new StatData(Stat.LifeSteal, m_lifeSteal)}
            };
        }

        private set { }
    }
}
