using System;
using TMPro;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [Header("Elements")]
    protected Player m_player;
    protected EnemyMovement m_enemyMovement;

    [Header("Spawn Related")]
    [SerializeField] protected SpriteRenderer m_characterSpriteRenderer; // enemy sprite renderer
    [SerializeField] protected SpriteRenderer m_spawnIndicatorRenderer; // enemy animator
    [SerializeField] protected float m_spawnIndicatorScale = 1.2f; // scale of the spawn indicator
    [SerializeField] protected float m_spawnIndicatorDuration = 0.3f; // duration of the spawn indicator
    [SerializeField] protected int m_spawnIndicatorLoopCount = 4; // delay of the spawn indicator
    protected bool m_hasSpawned; // flag to check if the enemy has spawned
    [SerializeField] protected float m_playerDetectionDistance; // minimum distance to player
    protected Collider2D m_collider; // collider of the enemy

    [Header("Effects")]
    [SerializeField] protected ParticleSystem m_enemyDeathEffectPrefab; // prefab to spawn when enemy dies

    [Header("Health")]
    [SerializeField] protected float m_maxHealth;
    [SerializeField] protected float m_currentHealth;
    protected TMP_Text m_healthText; // health text to display the current health

    [Header("Actions")]
    public static Action<float, Vector2> onDamageTaken; // action to notify when the enemy is damaged

    [Header("DEBUG")]
    [SerializeField] protected bool m_isGizmosEnabled;

    protected virtual void Awake()
    {
        m_collider = GetComponent<Collider2D>(); // get the collider of the enemy
        m_enemyMovement = GetComponent<EnemyMovement>(); // get the enemy movement script
        m_healthText = GetComponentInChildren<TMP_Text>(); // Find the health text in the scene
        m_player = FindFirstObjectByType<Player>();

        if (m_player == null)
        {
            Debug.LogWarning("No Player Found. Auto-destroying...");
            Destroy(gameObject);
        }
    }
    protected virtual void Start()
    {
        m_currentHealth = m_maxHealth; // Set the initial health to max health

        m_collider.enabled = false; // disable the collider until the spawn sequence is completed

        m_enemyMovement.enabled = false; // disable the enemy movement script until the spawn sequence is completed

        if (m_healthText != null) // Check if health text is assigned
        {
            m_healthText.text = m_currentHealth.ToString(); // Set the initial health text
        }

        StartSpawnSequence();
    }

    protected void SetRendererVisiblity(bool isVisible)
    {
        m_characterSpriteRenderer.enabled = isVisible; // hide the character renderer
        m_spawnIndicatorRenderer.enabled = !isVisible; // show the spawn indicator renderer
    }

    protected void StartSpawnSequence()
    {
        SetRendererVisiblity(false); // hide the character renderer

        Vector3 targetScale = m_spawnIndicatorRenderer.transform.localScale * m_spawnIndicatorScale; // get the target scale of the spawn indicator
        LeanTween.scale(m_spawnIndicatorRenderer.gameObject, targetScale, m_spawnIndicatorDuration)
        .setLoopPingPong(m_spawnIndicatorLoopCount)
        .setOnComplete(SpawnSequenceCompleted);
    }

    protected void SpawnSequenceCompleted()
    {
        SetRendererVisiblity(true); // show the character renderer

        m_hasSpawned = true; // set the spawn flag to true

        // enable the collider after the spawn sequence is completed. Because we don't want the enemy to be found by the weapon, so that we don't attack them.
        m_collider.enabled = true;

        m_enemyMovement.enabled = true; // enable the enemy movement script
        m_enemyMovement.StorePlayer(m_player); // set the player reference in the enemy movement script
    }

    protected void PlayDeathEffect()
    {
        m_enemyDeathEffectPrefab.transform.SetParent(null); // detach the effect from the enemy
        m_enemyDeathEffectPrefab.Play(); // play the death effect at the enemy's position
    }

    public void TakeDamage(float damage)
    {
        float realDamage = Mathf.Clamp(damage, 0, m_currentHealth); // Ensure damage doesn't exceed current health

        m_currentHealth -= realDamage; // Reduce current health by damage taken

        if (m_healthText != null) // Check if health text is assigned
        {
            m_healthText.text = m_currentHealth.ToString();
        }

        onDamageTaken?.Invoke(realDamage, transform.position); // Notify that the enemy has been damaged

        if (m_currentHealth <= 0)
        {
            PassAway();
        }
    }

    protected void PassAway()
    {
        PlayDeathEffect(); // play the death effect
        Destroy(gameObject); // destroy the enemy when it is close to player
    }

    protected void ManageAttack()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, m_player.transform.position); // get the distance to player

        if (distanceToPlayer > m_playerDetectionDistance)
        {
            m_enemyMovement.FollowPlayer();
        }
        else
        {
            TryAttack();
        }
    }

    protected abstract void TryAttack();

    protected void OnDrawGizmos()
    {
        if (m_isGizmosEnabled == false)
        {
            return;
        }

        Gizmos.color = Color.red; // Set the color of the Gizmos to red

        Gizmos.DrawWireSphere(transform.position, m_playerDetectionDistance); // Draw a wire sphere at the enemy's position with the detection distance
    }
}
