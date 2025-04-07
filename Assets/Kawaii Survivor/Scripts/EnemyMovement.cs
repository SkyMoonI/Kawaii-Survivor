using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("Elements")]
    private Player m_player;

    [Header("Spawn Related")]
    [SerializeField] private SpriteRenderer m_characterSpriteRenderer; // enemy sprite renderer
    [SerializeField] private SpriteRenderer m_spawnIndicatorRenderer; // enemy animator
    [SerializeField] private float m_spawnIndicatorScale = 1.2f; // scale of the spawn indicator
    [SerializeField] private float m_spawnIndicatorDuration = 0.3f; // duration of the spawn indicator
    [SerializeField] private int m_spawnIndicatorLoopCount = 4; // delay of the spawn indicator
    private bool m_hasSpawned; // flag to check if the enemy has spawned

    [Header("Settings")]
    [SerializeField] private float m_moveSpeed;
    [SerializeField] private float m_playerDetectionDistance; // minimum distance to player

    [Header("Effects")]
    [SerializeField] private ParticleSystem m_enemyDeathEffectPrefab; // prefab to spawn when enemy dies

    [Header("DEBUG")]
    [SerializeField] private bool m_isGizmosEnabled;

    void Awake()
    {
        m_player = FindFirstObjectByType<Player>();

        if (m_player == null)
        {
            Debug.LogWarning("No Player Found. Auto-destroying...");
            Destroy(gameObject);
        }
    }

    void Start()
    {
        m_characterSpriteRenderer.enabled = false; // hide the character renderer
        m_spawnIndicatorRenderer.enabled = true; // show the spawn indicator renderer

        Vector3 targetScale = m_spawnIndicatorRenderer.transform.localScale * m_spawnIndicatorDuration; // get the target scale of the spawn indicator
        LeanTween.scale(m_spawnIndicatorRenderer.gameObject, targetScale, m_spawnIndicatorDuration)
        .setLoopPingPong(m_spawnIndicatorLoopCount)
        .setOnComplete(SpawnSequenceCompleted);

    }

    void Update()
    {
        if (m_hasSpawned == false) // check if the enemy has spawned
        {
            return; // if not, do nothing
        }

        FollowPlayer();
        TryAttack();
    }

    private void SpawnSequenceCompleted()
    {
        m_characterSpriteRenderer.enabled = true; // show the character renderer
        m_spawnIndicatorRenderer.enabled = false; // hide the spawn indicator renderer

        m_hasSpawned = true; // set the spawn flag to true
    }

    private void FollowPlayer()
    {
        Vector2 directionVector = m_player.transform.position - transform.position; // a vector from enemy to player
        Vector2 normalizedDirectionVector = directionVector.normalized; // normalize it to get raw vector

        // add position the raw vector with move speed to get target position
        Vector2 targetPosition = (Vector2)transform.position + normalizedDirectionVector * m_moveSpeed * Time.deltaTime;

        transform.position = targetPosition; // every frame we assign the new position
    }

    private void TryAttack()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, m_player.transform.position); // get the distance to player

        if (distanceToPlayer < m_playerDetectionDistance)
        {
            Debug.Log("Enemy is attacking the player!");

            PlayDeathEffect(); // play the death effect
            Destroy(gameObject); // destroy the enemy when it is close to player
        }
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
