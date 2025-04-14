using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private HealthBar m_healthBar;

    [Header("Settings")]
    [SerializeField] private float m_maxHealth = 100f;
    [SerializeField] private float m_currentHealth;

    void Start()
    {
        m_currentHealth = m_maxHealth; // Set the initial health to max health

        m_healthBar.UpdateHealth(m_currentHealth, m_maxHealth); // Update the health bar for initial health
    }

    public void TakeDamage(float damage)
    {
        float realDamage = Mathf.Clamp(damage, 0, m_currentHealth); // Ensure damage doesn't exceed current health

        m_currentHealth -= realDamage; // Reduce current health by damage taken

        m_healthBar.UpdateHealth(m_currentHealth, m_maxHealth); // Update the health bar

        if (m_currentHealth <= 0)
        {
            PassAway();
        }
    }

    private void PassAway()
    {
        GameManager.Instance.SetGameState(GameState.GAMEOVER); // Set the game state to game over
    }
}
