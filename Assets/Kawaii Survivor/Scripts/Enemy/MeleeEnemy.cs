
using TMPro;
using UnityEngine;

[RequireComponent(typeof(EnemyMovement))]
public class MeleeEnemy : Enemy
{
    [Header("Attack Settings")]
    [SerializeField] private float m_attackFrequency; // attack frequency in seconds
    [SerializeField] private int m_attackDamage; // damage dealt to the player
    private float m_attackDelay; // attack duration in seconds
    private float m_attackTimer; // attack range in units

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();

        m_attackDelay = 1f / m_attackFrequency; // calculate the attack time based on the frequency per second
    }

    void Update()
    {
        if (!m_hasSpawned) return;

        ManageAttack();

        transform.localScale = m_player.transform.position.x > transform.position.x ? Vector3.one : Vector3.one.With(x: -1); // flip the enemy sprite based on the player's scale

        WaitForAttack();
    }

    protected override void TryAttack()
    {
        if (m_attackTimer >= m_attackDelay)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, m_player.transform.position); // get the distance to player

            if (distanceToPlayer < m_playerDetectionDistance)
            {
                Attack();
            }
        }
    }
    private void Attack()
    {
        m_player.TakeDamage(m_attackDamage); // deal damage to the player

        m_attackTimer = 0f; // reset the attack delay
    }

    private void WaitForAttack()
    {
        m_attackTimer += Time.deltaTime; // increase the attack delay
    }




}
