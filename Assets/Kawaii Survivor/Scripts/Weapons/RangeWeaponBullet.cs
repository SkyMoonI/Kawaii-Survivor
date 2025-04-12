using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(CircleCollider2D))] // require a rigidbody component
public class RangeWeaponBullet : MonoBehaviour
{
    [Header("Elements")]
    private Rigidbody2D m_rigidBody; // reference to the rigidbody component
    private RangeWeapon m_rangeWeapon; // reference to the enemy attack script

    [Header("Settings")]
    [SerializeField] private LayerMask m_enemyLayerMask; // layer mask for the enemy layer
    private float m_damage; // speed of the bullet
    private Enemy m_target;
    private bool m_isCriticalHit; // flag to indicate if the hit is critical

    void Awake()
    {
        m_rigidBody = GetComponent<Rigidbody2D>(); // get the rigidbody component
    }

    void Start()
    {
        m_target = null; // initialize the target to null
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (m_target != null) // if the bullet has already hit an enemy, ignore further collisions
        {
            return;
        }

        //if (other.TryGetComponent(out Enemy enemy))
        //{
        //    if (enemy != null)
        //    {
        //        m_target = enemy; // set the target to the enemy that was hit
        //
        //        LeanTween.cancel(gameObject); // cancel the delayed call to deactivate the bullet
        //
        //        enemy.TakeDamage(m_damage); // Deal damage to the player
        //
        //        m_rangeWeapon.ReleaseBullet(this); // store the bullet in the pool
        //    }
        //}

        if (IsInLayerMask(other.gameObject.layer, m_enemyLayerMask))
        {
            m_target = other.GetComponent<Enemy>(); // set the target to the enemy that was hit
            LeanTween.cancel(gameObject); // cancel the delayed call to deactivate the bullet

            m_target.TakeDamage(m_damage, m_isCriticalHit); // Deal damage to the player

            m_rangeWeapon.ReleaseBullet(this); // store the bullet in the pool
        }
    }

    public void Configure(RangeWeapon rangeWeapon)
    {
        m_rangeWeapon = rangeWeapon; // set the enemy attack script reference
    }

    public void Shoot(float damage, float speed, Vector2 direction, bool isCriticalHit)
    {
        LeanTween.delayedCall(gameObject, 3f, () => m_rangeWeapon.ReleaseBullet(this)); // deactivate the bullet after a short delay

        m_isCriticalHit = isCriticalHit;
        m_damage = damage; // set the bullet damage

        transform.right = direction; // set the bullet's rotation to face the player

        m_rigidBody.linearVelocity = direction * speed; // set the bullet velocity
    }

    public void Reload(Vector2 bulletSpawnPoint)
    {
        m_target = null; // reset the target

        transform.position = bulletSpawnPoint; // set the bullet position to the spawn point
        m_rigidBody.linearVelocity = Vector2.zero; // reset the bullet velocity
    }

    private bool IsInLayerMask(int layer, LayerMask layerMask)
    {
        return (layerMask.value & (1 << layer)) != 0; // check if the layer is in the layer mask
    }
}
