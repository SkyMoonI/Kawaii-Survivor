using UnityEngine;

[RequireComponent(typeof(EnemyMovement))]
public class Enemy : MonoBehaviour
{
    [Header("Elements")]
    private Player m_player;
    private EnemyMovement m_enemyMovement;

    [Header("Spawn Related")]
    [SerializeField] private SpriteRenderer m_characterSpriteRenderer; // enemy sprite renderer
    [SerializeField] private SpriteRenderer m_spawnIndicatorRenderer; // enemy animator
    [SerializeField] private float m_spawnIndicatorScale = 1.2f; // scale of the spawn indicator
    [SerializeField] private float m_spawnIndicatorDuration = 0.3f; // duration of the spawn indicator
    [SerializeField] private int m_spawnIndicatorLoopCount = 4; // delay of the spawn indicator
    private bool m_hasSpawned; // flag to check if the enemy has spawned
    [SerializeField] private float m_playerDetectionDistance; // minimum distance to player

    [Header("Effects")]
    [SerializeField] private ParticleSystem m_enemyDeathEffectPrefab; // prefab to spawn when enemy dies

    [Header("Attack Settings")]
    [SerializeField] private float m_attackFrequency; // attack frequency in seconds
    private float m_attackDelay; // attack duration in seconds
    private float m_attackTimer; // attack range in units
    [SerializeField] private float m_attackDamage = 10f; // damage dealt to the player


    [Header("DEBUG")]
    [SerializeField] private bool m_isGizmosEnabled;

    void Awake()
    {
        m_player = FindFirstObjectByType<Player>();

        m_enemyMovement = GetComponent<EnemyMovement>();
        m_enemyMovement.enabled = false; // disable the enemy movement script until the spawn sequence is completed

        m_attackDelay = 1f / m_attackFrequency; // calculate the attack time based on the frequency per second

        if (m_player == null)
        {
            Debug.LogWarning("No Player Found. Auto-destroying...");
            Destroy(gameObject);
        }
    }

    void Start()
    {
        StartSpawnSequence();
    }

    void Update()
    {
        if (!m_hasSpawned) return;

        if (m_attackTimer >= m_attackDelay)
        {
            TryAttack();
        }
        else
        {
            WaitForAttack();
        }
    }

    private void SetRendererVisiblity(bool isVisible)
    {
        m_characterSpriteRenderer.enabled = isVisible; // hide the character renderer
        m_spawnIndicatorRenderer.enabled = !isVisible; // show the spawn indicator renderer
    }

    private void StartSpawnSequence()
    {
        SetRendererVisiblity(false); // hide the character renderer

        Vector3 targetScale = m_spawnIndicatorRenderer.transform.localScale * m_spawnIndicatorDuration; // get the target scale of the spawn indicator
        LeanTween.scale(m_spawnIndicatorRenderer.gameObject, targetScale, m_spawnIndicatorDuration)
        .setLoopPingPong(m_spawnIndicatorLoopCount)
        .setOnComplete(SpawnSequenceCompleted);
    }

    private void SpawnSequenceCompleted()
    {
        SetRendererVisiblity(true); // show the character renderer

        m_hasSpawned = true; // set the spawn flag to true

        m_enemyMovement.enabled = true; // enable the enemy movement script
        m_enemyMovement.SetPlayer(m_player); // set the player reference in the enemy movement script
    }

    private void TryAttack()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, m_player.transform.position); // get the distance to player

        if (distanceToPlayer < m_playerDetectionDistance)
        {
            Attack();
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

    private void Die()
    {
        PlayDeathEffect(); // play the death effect
        Destroy(gameObject); // destroy the enemy when it is close to player
    }

    private void PlayDeathEffect()
    {
        m_enemyDeathEffectPrefab.transform.SetParent(null); // detach the effect from the enemy
        m_enemyDeathEffectPrefab.Play(); // play the death effect at the enemy's position
    }

    void OnDrawGizmos()
    {
        if (m_isGizmosEnabled == false)
        {
            return;
        }

        Gizmos.color = Color.red; // Set the color of the Gizmos to red

        Gizmos.DrawWireSphere(transform.position, m_playerDetectionDistance); // Draw a wire sphere at the enemy's position with the detection distance
    }
}
