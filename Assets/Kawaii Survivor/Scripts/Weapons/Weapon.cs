using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Weapon : MonoBehaviour, IPlayerStatsDependency
{
    [Header("Scriptable Objects")]
    [field: SerializeField] private WeaponDataSO m_weaponData; // Reference to weapon data scriptable object 
    public WeaponDataSO WeaponData => m_weaponData;

    [Header("Settings")]
    protected float m_enemyDetectionRange;
    [SerializeField] protected LayerMask m_enemyMask; // Layer mask to filter enemies
    protected List<Enemy> m_damagedEnemies = new List<Enemy>(); // List of enemies that have been damaged by the weapon

    [Header("Attack Damage Settings")]
    protected float m_baseDamage; // base Damage dealt by the weapon
    protected float m_currentDamage; // Current damage dealt by the weapon

    [Header("Attack Speed Settings")]
    protected float m_baseAttackFrequency; // base attack frequency in seconds
    protected float m_currentAttackFrequency; // attack frequency in seconds
    protected float m_attackDelay; // attack duration in seconds
    protected float m_attackTimer; // attack range in units

    [Header("Range Settings")]
    protected float m_baseAttackRange; // attack range in units
    protected float m_currentAttackRange; // attack range in units

    [Header("Critical Hit Settings")]
    protected float m_baseCriticalHitChance; // base chance to deal critical hit
    protected float m_currentCriticalHitChance; // chance to deal critical hit
    protected float m_baseCriticalPercent; // multiplier for critical hit damage
    protected float m_currentCriticalPercent; // multiplier for critical hit damage

    [Header("Animations")]
    [SerializeField] protected float m_aimLerp; // Lerp speed for aiming
    protected Vector2 m_targetUpVector; // Target up vector for aiming

    [Header("Leveling")]
    [SerializeField] protected int m_level; // Current level of the weapon
    public int Level => m_level;

    [Header("DEBUG")]
    [SerializeField] protected bool m_isGizmosEnabled;

    protected void Update()
    {
        CleanAim();
    }

    private void CleanAim()
    {
        transform.up = Vector3.Lerp(transform.up, m_targetUpVector, m_aimLerp * Time.deltaTime); // Set the weapon's up direction to the world up direction

    }
    protected void AutoAim()
    {
        Enemy closestEnemy = GetClosestEnemy(); // Get the closest enemy within detection range

        if (closestEnemy == null) // If no closest enemy is found, return
        {
            Vector2 defaultPosition = Vector3.up;
            m_targetUpVector = defaultPosition; // Set the target up vector to the default position
            // transform.up = Vector3.Lerp(transform.up, targetUpVector, m_aimLerp * Time.deltaTime); // Set the weapon's up direction to the world up direction
        }
        else
        {
            Vector2 directionToEnemy = (closestEnemy.transform.position - transform.position).normalized;
            m_targetUpVector = directionToEnemy; // Calculate the direction to the closest enemy
            // transform.up = targetUpVector; // Set the weapon's up direction to the direction of the closest enemy directly
            // transform.up = Vector3.Lerp(transform.up, targetUpVector, m_aimLerp * Time.deltaTime); // Smoothly rotate the weapon towards the closest enemy
            ManageAttack();
        }

        WaitForAttack(); // Wait for the attack delay for every frame except while attacking
    }

    protected Enemy GetClosestEnemy()
    {
        Enemy closestEnemy = null;
        float closestDistance = m_enemyDetectionRange; // Initialize the closest distance to infinity

        Enemy[] enemies = Physics2D.OverlapCircleAll(transform.position, m_enemyDetectionRange, m_enemyMask) // Find all enemies within the detection range
            .Select(collider => collider.GetComponent<Enemy>()) // Get the Enemy component from each collider
            .Where(enemy => enemy != null) // Filter out null enemies
            .ToArray(); // Convert to an array

        if (enemies.Length <= 0) // If no enemies are found within detection range
        {
            return null; // Return null
        }

        foreach (Enemy enemy in enemies)
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position); // Calculate the distance to the enemy

            if (distance < closestDistance) // Check if the enemy is within detection range and closer than the closest one found so far
            {
                closestDistance = distance; // Update the closest distance
                closestEnemy = enemy; // Update the closest enemy
            }
        }

        return closestEnemy;
    }
    protected abstract void StartAttack();

    protected void ManageAttack()
    {
        if (m_attackTimer >= m_attackDelay)
        {
            StartAttack();
            m_attackTimer = 0f; // Reset the attack timer
        }
    }

    protected void WaitForAttack()
    {
        m_attackTimer += Time.deltaTime; // increase the attack delay
    }

    protected float GetDamage(out bool isCriticalHit)
    {
        isCriticalHit = false; // Initialize isCriticalHit to false

        if (UnityEngine.Random.Range(0, 101) < m_currentCriticalHitChance) // 10% chance to deal critical hit
        {
            isCriticalHit = true; // Set isCriticalHit to true
            return m_currentDamage * m_currentCriticalPercent; // Return double damage for critical hit
        }

        // If not a critical hit, return normal damage
        return m_currentDamage;
    }

    protected virtual void OnDrawGizmos()
    {
        if (m_isGizmosEnabled == false)
        {
            return;
        }

        Gizmos.color = Color.blue; // Set the color of the Gizmos to red
        Gizmos.DrawWireSphere(transform.position, m_enemyDetectionRange); // Draw a wire sphere at the enemy's position with the detection distance);
    }

    private void ConfigureStats()
    {
        Dictionary<Stat, float> calculatedBaseStats = WeaponStatsCalculator.GetStats(m_weaponData, m_level); // Get the stats from the weapon data

        m_baseDamage = calculatedBaseStats[Stat.Attack]; // Set the base damage from the weapon data
        m_baseAttackFrequency = calculatedBaseStats[Stat.AttackSpeed]; // Set the attack frequency from the weapon data
        m_baseAttackRange = calculatedBaseStats[Stat.Range]; // Set the base attack range from the weapon data
        m_baseCriticalHitChance = calculatedBaseStats[Stat.CriticalChance]; // Set the base critical hit chance from the weapon data
        m_baseCriticalPercent = calculatedBaseStats[Stat.CriticalDamage]; // Set the base critical hit damage from the weapon data
    }

    public void UpdateStats(PlayerStatManager playerStatManager)
    {
        ConfigureStats(); // Configure the stats from the weapon data

        float addedDamage = 1 + (playerStatManager.GetStatValue(Stat.Attack) / 100f);
        m_currentDamage = m_baseDamage * addedDamage;
        m_currentDamage = Mathf.Max(m_currentDamage, 1);

        float addedAttackSpeed = 1 + (playerStatManager.GetStatValue(Stat.AttackSpeed) / 100f);
        m_currentAttackFrequency = m_baseAttackFrequency * addedAttackSpeed; // Set the attack frequency from the weapon data
        m_attackDelay = 1f / m_currentAttackFrequency; // calculate the attack time based on the frequency per second

        if (m_weaponData.Prefab.GetType() == typeof(RangeWeapon))
        {
            float addedAttackRange = 1 + (playerStatManager.GetStatValue(Stat.Range) / 100f);
            m_currentAttackRange = m_baseAttackRange * addedAttackRange; // Set the attack range from the weapon data
        }
        else if (m_weaponData.Prefab.GetType() == typeof(MeleeWeapon))
        {
            m_currentAttackRange = m_baseAttackRange; // Set the attack range from the weapon data
        }

        m_enemyDetectionRange = m_currentAttackRange; // Set the enemy detection range to the attack range

        float addedCriticalHitChance = 1 + (playerStatManager.GetStatValue(Stat.CriticalChance) / 100f);
        m_currentCriticalHitChance = m_baseCriticalHitChance * addedCriticalHitChance; // Set the attack range from the weapon data

        float addedCriticalHitDamage = 1 + (playerStatManager.GetStatValue(Stat.CriticalDamage) / 100f);
        m_currentCriticalPercent = m_baseCriticalPercent * addedCriticalHitDamage;
    }

    public void UpgradeTo(int level)
    {
        m_level = level; // Set the weapon level to the specified level
        UpdateStats(PlayerStatManager.Instance); // Update the stats based on the new level
    }
}
