
using TMPro;
using UnityEngine;

[RequireComponent(typeof(EnemyMovement)), RequireComponent(typeof(RangeEnemyAttack))]
public class RangeEnemy : Enemy
{
    [Header("Elements")]
    private RangeEnemyAttack m_rangeEnemyAttack; // reference to the enemy attack script

    protected override void Awake()
    {
        base.Awake();

        m_rangeEnemyAttack = GetComponent<RangeEnemyAttack>(); // get the enemy attack script
        m_rangeEnemyAttack.StorePlayer(m_player); // set the player reference in the enemy attack script
    }

    protected override void Start()
    {
        base.Start();
    }

    void Update()
    {
        if (!m_hasSpawned) return;

        ManageAttack();

        transform.localScale = m_player.transform.position.x > transform.position.x ? Vector3.one : Vector3.one.With(x: -1); // flip the enemy sprite based on the player's scale
    }

    protected override void TryAttack()
    {
        m_rangeEnemyAttack.AutoAim(); // call the attack method in the enemy attack script
    }
}
