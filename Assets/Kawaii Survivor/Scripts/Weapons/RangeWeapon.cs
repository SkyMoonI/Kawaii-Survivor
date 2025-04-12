using UnityEngine;
using UnityEngine.Pool;

public class RangeWeapon : Weapon
{
    [Header("Elements")]
    [SerializeField] private RangeWeaponBullet m_bulletPrefab; // reference to the bullet prefab
    [SerializeField] private Transform m_bulletSpawnPoint; // reference to the bullet spawn point
    [SerializeField] private GameObject m_bulletObjectHolder; // takes the initiated bullets to avoid cluttering the scene

    [Header("Attack Settings")]
    [SerializeField] private float m_bulletSpeed = 10f; // speed of the bullet

    [Header("Pooling")]
    private ObjectPool<RangeWeaponBullet> m_rangeWeaponBulletPool; // pool to store the damage text prefabs

    [Header("DEBUG")]
    private Vector2 m_gizmosAttackDirection;

    void Awake()
    {
        m_rangeWeaponBulletPool = new ObjectPool<RangeWeaponBullet>(CreateBullet, OnGetBullet, OnReleaseBullet, OnDestroyBullet, false, 10);
    }

    new void Update()
    {
        base.Update(); // Call the base class Update method

        AutoAim(); // aim at the closest enemy
    }

    private RangeWeaponBullet CreateBullet()
    {
        RangeWeaponBullet bulletInstance = Instantiate(m_bulletPrefab, m_bulletSpawnPoint.position, Quaternion.identity, m_bulletObjectHolder.transform); // set the parent to this object

        bulletInstance.Configure(this); // set the enemy attack script reference

        return bulletInstance;
    }

    private void OnGetBullet(RangeWeaponBullet bullet)
    {
        bullet.Reload(m_bulletSpawnPoint.position); // reset the bullet instance

        bullet.gameObject.SetActive(true);
    }
    private void OnReleaseBullet(RangeWeaponBullet bullet)
    {
        bullet.gameObject.SetActive(false);
    }
    private void OnDestroyBullet(RangeWeaponBullet bullet)
    {
        Destroy(bullet.gameObject);
    }

    public void ReleaseBullet(RangeWeaponBullet bullet)
    {
        m_rangeWeaponBulletPool.Release(bullet); // store the bullet in the pool
    }

    protected override void StartAttack()
    {
        Shoot();
    }

    private void Shoot()
    {
        Vector2 directionToEnemy = (GetClosestEnemy().GetCenterPosition() - (Vector2)m_bulletSpawnPoint.position).normalized; // get the direction to the player
        m_gizmosAttackDirection = directionToEnemy; // store the direction for gizmos

        RangeWeaponBullet bulletInstance = m_rangeWeaponBulletPool.Get(); // instantiate the bullet prefab

        float damage = GetDamage(out bool isCriticalHit); // Get the damage value from the weapon

        bulletInstance.Shoot(damage, m_bulletSpeed, directionToEnemy, isCriticalHit); // shoot the bullet with the specified damage and speed
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos(); // Call the base class OnDrawGizmos method

        Gizmos.color = Color.white; // Set the color of the Gizmos to red

        Gizmos.DrawLine(transform.position, (Vector2)m_bulletSpawnPoint.position + m_gizmosAttackDirection * 5); // Draw a wire sphere at the enemy's position with the detection distance
    }


}
