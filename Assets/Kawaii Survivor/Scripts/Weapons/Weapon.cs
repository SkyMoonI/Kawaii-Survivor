using System.Linq;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Settings")]
    private float m_damage; // Damage dealt by the weapon
    [SerializeField] private float m_enemyDetectionRange;
    [SerializeField] private LayerMask m_enemyMask; // Layer mask to filter enemies

    [Header("Animations")]
    [SerializeField] private float m_aimLerp; // Lerp speed for aiming

    [Header("DEBUG")]
    [SerializeField] private bool m_isGizmosEnabled;

    // Update is called once per frame
    void Update()
    {
        AutoAim();
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
        }
    }

    private Enemy GetClosestEnemy()
    {
        Enemy closestEnemy = null;
        float closestDistance = m_enemyDetectionRange; // Initialize the closest distance to infinity

        // Enemy[] enemies = FindObjectsByType<Enemy>(FindObjectsInactive.Exclude, FindObjectsSortMode.None); // Find all enemies in the scene
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

    void OnDrawGizmos()
    {
        if (m_isGizmosEnabled == false)
        {
            return;
        }

        Gizmos.color = Color.blue; // Set the color of the Gizmos to red

        Gizmos.DrawWireSphere(transform.position, m_enemyDetectionRange); // Draw a wire sphere at the enemy's position with the detection distance);
    }
}
