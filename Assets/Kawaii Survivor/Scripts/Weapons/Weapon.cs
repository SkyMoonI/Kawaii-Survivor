using System;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] protected float m_enemyDetectionRange;
    [SerializeField] protected LayerMask m_enemyMask; // Layer mask to filter enemies
    protected List<Enemy> m_damagedEnemies = new List<Enemy>(); // List of enemies that have been damaged by the weapon

    [Header("Attack Settings")]
    [SerializeField] protected float m_damage; // Damage dealt by the weapon
    [SerializeField] protected float m_attackFrequency; // attack frequency in seconds
    protected float m_attackDelay; // attack duration in seconds
    protected float m_attackTimer; // attack range in units
    [Range(0, 100)]
    [SerializeField] protected float m_criticalHitChance; // chance to deal critical hit
    [SerializeField] protected float m_criticalHitMultiplier; // multiplier for critical hit damage

    [Header("Animations")]
    [SerializeField] protected float m_aimLerp; // Lerp speed for aiming
    protected Vector2 m_targetUpVector; // Target up vector for aiming

    [Header("DEBUG")]
    [SerializeField] protected bool m_isGizmosEnabled;

    void Start()
    {
        m_attackDelay = 1f / m_attackFrequency; // calculate the attack time based on the frequency per second
    }

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

        if (UnityEngine.Random.Range(0, 101) < m_criticalHitChance) // 10% chance to deal critical hit
        {
            isCriticalHit = true; // Set isCriticalHit to true
            return m_damage * m_criticalHitMultiplier; // Return double damage for critical hit
        }

        // If not a critical hit, return normal damage
        return m_damage;
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
}
