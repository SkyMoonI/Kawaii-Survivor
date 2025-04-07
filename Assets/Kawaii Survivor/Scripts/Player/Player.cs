using UnityEngine;

[RequireComponent(typeof(PlayerHealth))]
[RequireComponent(typeof(PlayerController))]
public class Player : MonoBehaviour
{
    [Header("Elements")]
    private PlayerHealth m_playerHealth;

    void Awake()
    {
        m_playerHealth = GetComponent<PlayerHealth>();
    }

    public void TakeDamage(float damage)
    {
        m_playerHealth.TakeDamage(damage);
    }
}
