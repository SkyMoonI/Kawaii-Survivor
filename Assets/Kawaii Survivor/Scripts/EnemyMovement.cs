using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("Elements")]
    private Player m_player;

    [Header("Settings")]
    [SerializeField] private float m_moveSpeed;
    [SerializeField] private float m_playerDetectionDistance; // minimum distance to player

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

    void Update()
    {
        FollowPlayer();

        TryAttack();
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
            Destroy(gameObject); // destroy the enemy when it is close to player
        }
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
