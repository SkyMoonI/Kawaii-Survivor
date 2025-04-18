using UnityEngine;
using UnityEngine.Pool;

public class RangeEnemyAttack : MonoBehaviour
{
    [Header("Elements")]
    private Player m_player; // reference to the player
    [SerializeField] private EnemyBullet m_bulletPrefab; // reference to the bullet prefab
    [SerializeField] private Transform m_bulletSpawnPoint; // reference to the bullet spawn point
    private GameObject m_bulletObjectHolder; // takes the initiated bullets to avoid cluttering the scene


    [Header("Attack Settings")]
    [SerializeField] private float m_attackFrequency; // attack frequency in seconds
    [SerializeField] private float m_attackDamage; // damage dealt to the player
    [SerializeField] private float m_bulletSpeed = 10f; // speed of the bullet
    private float m_attackDelay; // attack duration in seconds
    private float m_attackTimer; // attack range in units

    [Header("Pooling")]
    private ObjectPool<EnemyBullet> m_enemyBulletPool; // pool to store the damage text prefabs

    [Header("DEBUG")]
    [SerializeField] private bool m_isGizmosEnabled;
    private Vector2 m_gizmosAttackDirection;

    void Awake()
    {
        m_bulletObjectHolder = GameObject.Find("Enemy Bullet Object Holder"); // find the bullet object holder in the scene
        m_enemyBulletPool = new ObjectPool<EnemyBullet>(CreateBullet, OnGetBullet, OnReleaseBullet, OnDestroyBullet, false, 10);
    }
    void Start()
    {
        m_attackDelay = 1f / m_attackFrequency; // calculate the attack time based on the frequency per second
    }

    private EnemyBullet CreateBullet()
    {
        EnemyBullet bulletInstance = Instantiate(m_bulletPrefab, m_bulletSpawnPoint.position, Quaternion.identity, m_bulletObjectHolder.transform); // set the parent to this object

        bulletInstance.Configure(this); // set the enemy attack script reference

        return bulletInstance;
    }

    private void OnGetBullet(EnemyBullet bullet)
    {
        bullet.Reload(m_bulletSpawnPoint.position); // reset the bullet instance

        bullet.gameObject.SetActive(true);
    }
    private void OnReleaseBullet(EnemyBullet bullet)
    {
        bullet.gameObject.SetActive(false);
    }
    private void OnDestroyBullet(EnemyBullet bullet)
    {
        Destroy(bullet.gameObject);
    }

    public void ReleaseBullet(EnemyBullet bullet)
    {
        m_enemyBulletPool.Release(bullet); // store the bullet in the pool
    }

    public void StorePlayer(Player player)
    {
        m_player = player; // store the reference to the player
    }

    public void AutoAim()
    {
        ManageShooting();
    }

    private void ManageShooting()
    {
        WaitForAttack(); // wait for the attack delay

        if (m_attackTimer >= m_attackDelay) // check if the attack delay is over
        {
            Shoot(); // shoot the bullet
            m_attackTimer = 0f; // reset the attack delay
        }
    }

    private void Shoot()
    {
        Vector2 directionToPlayer = (m_player.GetCenterPosition() - (Vector2)m_bulletSpawnPoint.position).normalized; // get the direction to the player
        m_gizmosAttackDirection = directionToPlayer; // store the direction for gizmos

        EnemyBullet bulletInstance = m_enemyBulletPool.Get(); // instantiate the bullet prefab

        bulletInstance.Shoot(m_attackDamage, m_bulletSpeed, directionToPlayer); // shoot the bullet with the specified damage and speed
    }

    private void WaitForAttack()
    {
        m_attackTimer += Time.deltaTime; // increase the attack delay
    }

    void OnDrawGizmos()
    {
        if (m_isGizmosEnabled == false)
        {
            return;
        }

        Gizmos.color = Color.white; // Set the color of the Gizmos to red

        Gizmos.DrawLine(transform.position, (Vector2)m_bulletSpawnPoint.position + m_gizmosAttackDirection * 5); // Draw a wire sphere at the enemy's position with the detection distance
    }
}
