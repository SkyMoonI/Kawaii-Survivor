using UnityEngine;

[RequireComponent(typeof(PlayerHealth))]
[RequireComponent(typeof(PlayerController))]
public class Player : MonoBehaviour
{
    [Header("Elements")]
    private PlayerHealth m_playerHealth;
    private CircleCollider2D m_playerCollider; // reference to the player collider

    void Awake()
    {
        m_playerHealth = GetComponent<PlayerHealth>();
        m_playerCollider = GetComponent<CircleCollider2D>(); // get the CircleCollider2D component
    }

    public void TakeDamage(float damage)
    {
        m_playerHealth.TakeDamage(damage);
    }

    public Vector2 GetCenterPosition()
    {
        return m_playerCollider.bounds.center; // get the center position of the player collider
    }
}
