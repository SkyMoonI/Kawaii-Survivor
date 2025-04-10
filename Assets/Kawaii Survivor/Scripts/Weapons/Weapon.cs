using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    private enum State
    {
        Idle,
        Attack
    }

    [Header("Elements")]
    [SerializeField] private Transform m_hitDetectionTransform; // The point where the weapon fires from
    private BoxCollider2D m_hitDetectionCollider; // The collider used for hit detection

    [Header("Settings")]
    [SerializeField] private float m_enemyDetectionRange;
    [SerializeField] private float m_hitDetectionRange;
    [SerializeField] private LayerMask m_enemyMask; // Layer mask to filter enemies
    private List<Enemy> m_damagedEnemies = new List<Enemy>(); // List of enemies that have been damaged by the weapon

    [Header("Attack Settings")]
    [SerializeField] private float m_damage; // Damage dealt by the weapon
    [SerializeField] private float m_attackFrequency; // attack frequency in seconds
    private float m_attackDelay; // attack duration in seconds
    private float m_attackTimer; // attack range in units

    [Header("Animations")]
    [SerializeField] private float m_aimLerp; // Lerp speed for aiming
    private State m_state; // Current state of the weapon
    private Animator m_animator; // Animator component for weapon animations

    [Header("DEBUG")]
    [SerializeField] private bool m_isGizmosEnabled;

    void Awake()
    {
        m_animator = GetComponent<Animator>(); // Get the Animator component attached to the weapon
        m_hitDetectionCollider = m_hitDetectionTransform.GetComponent<BoxCollider2D>(); // Get the BoxCollider2D component attached to the hit detection transform
    }
    void Start()
    {
        m_state = State.Idle; // Set the initial state to Idle
        m_attackDelay = 1f / m_attackFrequency; // calculate the attack time based on the frequency per second
        m_animator.speed = m_attackFrequency;
    }
    // Update is called once per frame
    void Update()
    {
        switch (m_state)
        {
            case State.Idle:
                AutoAim();
                break;
            case State.Attack:
                Attacking(); // Call the attacking method
                break;
        }

    }

    private void AutoAim()
    {
        Enemy closestEnemy = GetClosestEnemy(); // Get the closest enemy within detection range
        Vector2 targetUpVector = Vector3.up;

        if (closestEnemy == null) // If no closest enemy is found, return
        {
            transform.up = Vector3.Lerp(transform.up, targetUpVector, m_aimLerp * Time.deltaTime); // Set the weapon's up direction to the world up direction
        }
        else
        {
            targetUpVector = (closestEnemy.transform.position - transform.position).normalized; // Calculate the direction to the closest enemy
            transform.up = Vector3.Lerp(transform.up, targetUpVector, m_aimLerp * Time.deltaTime); // Smoothly rotate the weapon towards the closest enemy
            ManageAttack();
        }

        WaitForAttack(); // Wait for the attack delay for every frame except while attacking
    }

    private Enemy GetClosestEnemy()
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

    private void ManageAttack()
    {
        if (m_attackTimer >= m_attackDelay)
        {
            StartAttack();
            m_attackTimer = 0f; // Reset the attack timer
        }
    }

    private void WaitForAttack()
    {
        m_attackTimer += Time.deltaTime; // increase the attack delay
    }

    [NaughtyAttributes.Button]
    private void StartAttack()
    {
        m_animator.Play("Attack"); // Play the attack animation
        m_state = State.Attack; // Set the state to Attack
        m_damagedEnemies.Clear(); // Clear the list of damaged enemies
    }

    private void Attacking()
    {
        Attack();
    }

    private void StopAttack()
    {
        m_state = State.Idle; // Set the state to Idle
        m_damagedEnemies.Clear(); // Clear the list of damaged enemies
    }

    private void Attack()
    {
        Enemy[] enemies = Physics2D.OverlapBoxAll
        (
          m_hitDetectionTransform.position,
          m_hitDetectionCollider.bounds.size,
          m_hitDetectionTransform.rotation.eulerAngles.z,
          m_enemyMask
        ) // Find all enemies within the detection range
           .Select(collider => collider.GetComponent<Enemy>()) // Get the Enemy component from each collider
           .Where(enemy => enemy != null) // Filter out null enemies
           .ToArray(); // Convert to an array


        if (enemies.Length <= 0) // If no enemies are found within detection range
        {
            return; // Return null
        }

        foreach (Enemy enemy in enemies)
        {
            if (!m_damagedEnemies.Contains(enemy)) // Check if the enemy has already been damaged
            {
                m_damagedEnemies.Add(enemy); // Add the enemy to the list of damaged enemies to avoid double damage
                enemy.TakeDamage(m_damage);
            }
        }
    }

    void OnDrawGizmos()
    {
        if (m_isGizmosEnabled == false)
        {
            return;
        }

        Gizmos.color = Color.blue; // Set the color of the Gizmos to red
        Gizmos.DrawWireSphere(transform.position, m_enemyDetectionRange); // Draw a wire sphere at the enemy's position with the detection distance);

        Gizmos.color = Color.red; // Set the color of the Gizmos to red
        Gizmos.DrawWireSphere(m_hitDetectionTransform.position, m_hitDetectionRange); // Draw a wire sphere at the hit detection point with the hit detection distance
    }
}
