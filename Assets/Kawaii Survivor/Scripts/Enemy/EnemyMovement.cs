using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("Elements")]
    private Player m_player;

    [Header("Settings")]
    [SerializeField] private float m_moveSpeed;

    void Update()
    {
        if (m_player != null)
        {
            FollowPlayer();
        }
    }

    public void SetPlayer(Player player)
    {
        m_player = player;
    }

    private void FollowPlayer()
    {
        Vector2 directionVector = m_player.transform.position - transform.position; // a vector from enemy to player
        Vector2 normalizedDirectionVector = directionVector.normalized; // normalize it to get raw vector

        // add position the raw vector with move speed to get target position
        Vector2 targetPosition = (Vector2)transform.position + normalizedDirectionVector * m_moveSpeed * Time.deltaTime;

        transform.position = targetPosition; // every frame we assign the new position
    }



}
