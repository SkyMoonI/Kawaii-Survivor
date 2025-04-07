using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [Header("Elements")]
    private HealthBar m_healthBar;

    [Header("Settings")]
    [SerializeField] private float m_maxHealth = 100f;
    [SerializeField] private float m_currentHealth;

    void Awake()
    {
        m_healthBar = FindFirstObjectByType<HealthBar>();
    }

    void Start()
    {
        m_currentHealth = m_maxHealth; // Set the initial health to max health

        m_healthBar.UpdateHealth(m_currentHealth, m_maxHealth); // Update the health bar for initial health
    }

    public void TakeDamage(float damage)
    {
        float realDamage = Mathf.Clamp(damage, 0, m_currentHealth); // Ensure damage doesn't exceed current health

        Debug.Log($"Player took {realDamage} damage.");

        m_currentHealth -= realDamage; // Reduce current health by damage taken

        m_healthBar.UpdateHealth(m_currentHealth, m_maxHealth); // Update the health bar

        if (m_currentHealth <= 0)
        {
            PassAway();
        }
    }

    private void PassAway()
    {
        // Implement the logic to pass away here
        Debug.Log("Player has passed away.");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reload the current scene
    }
}
