using UnityEngine;
using UnityEngine.Pool;

public class DamageTextManager : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private DamageText m_damageTextPrefab; // prefab to spawn when enemy dies

    [Header("Pooling")]
    private ObjectPool<DamageText> m_damageTextPool; // pool to store the damage text prefabs

    private void Awake()
    {
        // Create a new pool for the damage text prefab
        m_damageTextPool = new ObjectPool<DamageText>(CreateDamageText, OnGetDamageText, OnReleaseDamageText, OnDestroyDamageText, false, 10);
    }

    void OnEnable()
    {
        Enemy.onDamageTaken += DamageTextCallBack;
        PlayerHealth.onAttackDodged += AttackDodgedCallback; // Subscribe to the attack dodged event
    }
    void OnDisable()
    {
        Enemy.onDamageTaken -= DamageTextCallBack;
        PlayerHealth.onAttackDodged -= AttackDodgedCallback; // Unsubscribe from the attack dodged event
    }
    void OnDestroy()
    {
        Enemy.onDamageTaken -= DamageTextCallBack;
        PlayerHealth.onAttackDodged -= AttackDodgedCallback; // Unsubscribe from the attack dodged event
    }

    private DamageText CreateDamageText()
    {
        // Instantiate a new damage text prefab and return it
        DamageText damageTextInstance = Instantiate(m_damageTextPrefab, transform); // set the parent to this object
        return damageTextInstance;
    }

    private void OnGetDamageText(DamageText damageTextInstance)
    {
        // Activate the damage text instance and set it to be a child of this object
        damageTextInstance.gameObject.SetActive(true);
    }
    private void OnReleaseDamageText(DamageText damageTextInstance)
    {
        // Deactivate the damage text instance and return it to the pool
        damageTextInstance.gameObject.SetActive(false);
    }
    private void OnDestroyDamageText(DamageText damageTextInstance)
    {
        // Destroy the damage text instance
        Destroy(damageTextInstance.gameObject);
        LeanTween.cancel(gameObject); // cancel the delayed call to deactivate the bullet
    }

    private void DamageTextCallBack(float damage, Vector2 enemyPosition, bool isCriticalHit)
    {
        DamageText damageTextInstance = m_damageTextPool.Get(); // get a damage text instance from the pool

        Vector2 spawnPosition = enemyPosition + Vector2.up * 1.5f; // spawn position above the enemy
        damageTextInstance.transform.position = spawnPosition; // set the position of the damage text instance

        damageTextInstance.Animate(damage.ToString(), isCriticalHit); // Call the Animate method to play the animation

        LeanTween.delayedCall(gameObject, 1f, () => { m_damageTextPool.Release(damageTextInstance); }); // release the damage text instance back to the pool after 1 second
    }

    private void AttackDodgedCallback(Vector2 playerPosition)
    {
        DamageText damageTextInstance = m_damageTextPool.Get(); // get a damage text instance from the pool

        Vector2 spawnPosition = playerPosition + Vector2.up * 1.5f; // spawn position above the enemy
        damageTextInstance.transform.position = spawnPosition; // set the position of the damage text instance

        damageTextInstance.Animate("Dodged", false); // Call the Animate method to play the animation

        LeanTween.delayedCall(gameObject, 1f, () => { m_damageTextPool.Release(damageTextInstance); }); // release the damage text instance back to the pool after 1 second
    }
}
