using UnityEngine;

public class RangeWeaponBullet : MonoBehaviour
{
    [Header("Elements")]
    private Rigidbody2D m_rigidBody; // reference to the rigidbody component
    private RangeWeapon m_rangeWeapon; // reference to the enemy attack script


    [Header("Settings")]
    private float m_damage; // speed of the bullet

    void Awake()
    {
        m_rigidBody = GetComponent<Rigidbody2D>(); // get the rigidbody component
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out Enemy enemy))
        {
            if (enemy != null)
            {
                LeanTween.cancel(gameObject); // cancel the delayed call to deactivate the bullet

                enemy.TakeDamage(m_damage); // Deal damage to the player

                m_rangeWeapon.ReleaseBullet(this); // store the bullet in the pool
            }
        }
    }

    public void Configure(RangeWeapon rangeWeapon)
    {
        m_rangeWeapon = rangeWeapon; // set the enemy attack script reference
    }

    public void Shoot(float damage, float speed, Vector2 direction)
    {
        LeanTween.delayedCall(gameObject, 3f, () => m_rangeWeapon.ReleaseBullet(this)); // deactivate the bullet after a short delay

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
