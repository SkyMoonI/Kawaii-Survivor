using System.Linq;
using UnityEngine;

public class MeleeWeapon : Weapon
{
    private enum State
    {
        Idle,
        Attack
    }

    [Header("Elements")]
    [SerializeField] private Transform m_hitDetectionTransform; // The point where the weapon fires from
    private BoxCollider2D m_hitDetectionCollider; // The collider used for hit detection


    [Header("Animations")]
    private State m_state; // Current state of the weapon
    private Animator m_animator; // Animator component for weapon animations

    void Awake()
    {
        m_animator = GetComponent<Animator>(); // Get the Animator component attached to the weapon
        m_hitDetectionCollider = m_hitDetectionTransform.GetComponent<BoxCollider2D>(); // Get the BoxCollider2D component attached to the hit detection transform
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_state = State.Idle; // Set the initial state to Idle
        m_animator.speed = m_currentAttackFrequency;
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update(); // Call the base class Update method

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

    [NaughtyAttributes.Button]
    protected override void StartAttack()
    {
        m_animator.Play("Attack"); // Play the attack animation
        m_state = State.Attack; // Set the state to Attack
        m_damagedEnemies.Clear(); // Clear the list of damaged enemies
    }

    protected void Attacking()
    {
        Attack();
    }

    protected void StopAttack()
    {
        m_state = State.Idle; // Set the state to Idle
        m_damagedEnemies.Clear(); // Clear the list of damaged enemies
    }


    protected void Attack()
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
                float damage = GetDamage(out bool isCriticalHit); // Get the damage value from the weapon

                m_damagedEnemies.Add(enemy); // Add the enemy to the list of damaged enemies to avoid double damage
                enemy.TakeDamage(damage, isCriticalHit);
            }
        }
    }
}
