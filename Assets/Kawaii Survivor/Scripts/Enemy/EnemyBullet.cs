using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [Header("Elements")]
    private Rigidbody2D m_rigidBody; // reference to the rigidbody component
    private RangeEnemyAttack m_rangeEnemyAttack; // reference to the enemy attack script

    [Header("Settings")]
    private float m_damage; // speed of the bullet

    void Awake()
    {
        m_rigidBody = GetComponent<Rigidbody2D>(); // get the rigidbody component
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out Player player))
        {
            if (player != null)
            {
                LeanTween.cancel(gameObject); // cancel the delayed call to deactivate the bullet

                player.TakeDamage(m_damage); // Deal damage to the player

                m_rangeEnemyAttack.ReleaseBullet(this); // store the bullet in the pool
            }
        }
    }
    public void Configure(RangeEnemyAttack rangeEnemyAttack)
    {
        m_rangeEnemyAttack = rangeEnemyAttack; // set the enemy attack script reference
    }

    public void Shoot(float damage, float speed, Vector2 direction)
    {
        LeanTween.delayedCall(gameObject, 3f, () => m_rangeEnemyAttack.ReleaseBullet(this)); // deactivate the bullet after a short delay

        m_damage = damage; // set the bullet damage

        transform.right = direction; // set the bullet's rotation to face the player

        m_rigidBody.linearVelocity = direction * speed; // set the bullet velocity
    }

    public void Reload(Vector2 bulletSpawnPoint)
    {
        transform.position = bulletSpawnPoint; // set the bullet position to the spawn point
        m_rigidBody.linearVelocity = Vector2.zero; // reset the bullet velocity
    }
}
