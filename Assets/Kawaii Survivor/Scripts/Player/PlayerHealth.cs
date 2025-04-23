using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerHealth : MonoBehaviour, IPlayerStatsDependency
{
    [Header("Elements")]
    [SerializeField] private HealthBar m_healthBar;

    [Header("Health")]
    [SerializeField] private float m_baseHealth;
    private float m_maxHealth;
    private float m_currentHealth;

    [Header("Armor")]
    [SerializeField] private float m_baseArmor;
    private float m_currentArmor;

    [Header("Life Steal")]
    [SerializeField] private float m_baseLifeSteal; // base life steal percentage
    private float m_currentLifeSteal; // life steal percentage

    [Header("Dodge")]
    [SerializeField] private float m_baseDodgeChance; // base dodge chance percentage
    private float m_currentDodgeChance; // dodge chance percentage

    [Header("Health Regeneration")]
    [SerializeField] private float m_baseHealthRegenAmount; // health regeneration rate
    private float m_currentHealthRegenAmount; // current health regeneration rate
    private float m_healthRegenTimer; // timer for health regeneration
    private float m_healthRegenInterval = 1f; // interval for health regeneration

    [Header("Actions")]
    public static Action<Vector2> onAttackDodged;


    void OnEnable()
    {
        Enemy.onDamageTaken += EnemyTookDamageCallback; // Subscribe to the enemy damage event
    }
    void OnDisable()
    {
        Enemy.onDamageTaken -= EnemyTookDamageCallback; // Unsubscribe from the enemy damage event
    }
    void OnDestroy()
    {
        Enemy.onDamageTaken -= EnemyTookDamageCallback; // Unsubscribe from the enemy damage event
    }

    void Update()
    {
        if (GameState.GAME == GameManager.Instance.CurrentGameState)
        {
            ManageHealthRegenTimer(); // Manage the health regeneration timer
        }
    }

    public void TakeDamage(float damage)
    {
        bool isDodged = Random.Range(0f, 100f) < m_currentDodgeChance; // Check if the damage is dodged based on dodge chance
        if (isDodged)
        {
            onAttackDodged?.Invoke(transform.position); // Invoke the dodge event with the player's position
            return; // If dodged, exit the method without taking damage
        }

        float realDamage = Mathf.Clamp(damage * (1 - m_currentArmor / 1000), 0, m_currentHealth); // Ensure damage doesn't exceed current health

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

    public void UpdateStats(PlayerStatManager playerStatManager)
    {
        float addedHealth = playerStatManager.GetStatValue(Stat.MaxHealth); // Get the added health from the stat manager
        m_maxHealth = m_baseHealth + addedHealth; // Calculate the new max health
        m_maxHealth = Mathf.Max(m_maxHealth, 1); // Ensure max health is at least 1

        m_currentHealth = m_maxHealth; // Reset current health to max health
        m_healthBar.UpdateHealth(m_currentHealth, m_maxHealth); // Update the health bar with new values

        float addedArmor = playerStatManager.GetStatValue(Stat.Armor); // Get the added armor from the stat manager
        m_currentArmor = m_baseArmor + addedArmor; // Calculate the new armor value
        m_currentArmor = Mathf.Max(m_currentArmor, 0); // Ensure armor is not negative

        float addedLifeSteal = playerStatManager.GetStatValue(Stat.LifeSteal); // Get the added life steal from the stat manager
        m_currentLifeSteal = m_baseLifeSteal + addedLifeSteal; // Calculate the new life steal value
        m_currentLifeSteal = Mathf.Max(m_currentLifeSteal, 0); // Ensure life steal is not negative

        float addedDodgeChance = playerStatManager.GetStatValue(Stat.Dodge); // Get the added dodge chance from the stat manager
        m_currentDodgeChance = m_baseDodgeChance + addedDodgeChance; // Calculate the new dodge chance value

        float addedHealthRegen = playerStatManager.GetStatValue(Stat.HealthRegeneration); // Get the added health regen from the stat manager
        m_currentHealthRegenAmount = m_baseHealthRegenAmount + addedHealthRegen; // Calculate the new health regen value
    }

    public void StealHealth(float damageDealt)
    {
        if (m_currentHealth >= m_maxHealth)
        {
            return;
        }

        float healAmount = damageDealt * m_currentLifeSteal / 100f; // Calculate the heal amount based on damage dealt and life steal percentage
        m_currentHealth = Mathf.Min(m_currentHealth + healAmount, m_maxHealth); // Ensure current health does not exceed max health

        m_healthBar.UpdateHealth(m_currentHealth, m_maxHealth); // Update the health bar with new values
    }

    private void EnemyTookDamageCallback(float damageDealt, Vector2 enemyPosition, bool isCriticalHit)
    {
        StealHealth(damageDealt); // Call the method to steal health based on damage dealt
    }

    private void RegenHealth()
    {
        if (m_currentHealth < m_maxHealth)
        {
            m_currentHealth = Mathf.Min(m_currentHealth + m_currentHealthRegenAmount, m_maxHealth); // Ensure current health does not exceed max health

            m_healthBar.UpdateHealth(m_currentHealth, m_maxHealth); // Update the health bar with new values
        }
    }

    private void ManageHealthRegenTimer()
    {
        if (m_healthRegenTimer >= m_healthRegenInterval)
        {
            m_healthRegenTimer = 0f; // Reset the timer
            RegenHealth(); // Call the health regeneration method
        }
        else
        {
            m_healthRegenTimer += Time.deltaTime; // Increment the timer
        }
    }
}
