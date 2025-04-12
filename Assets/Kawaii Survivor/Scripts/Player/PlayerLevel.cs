using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLevel : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Slider m_slider;
    [SerializeField] private TMP_Text m_text;

    [Header("Settings")]
    [SerializeField] private int m_level = 1; // Current level of the player
    [SerializeField] private int m_levelUpMultiplier = 5; // Amount to increase level on each level up
    private int m_currentXp; // Current XP of the player
    private int m_requiredXp; // XP required to level up

    void OnEnable()
    {
        UpdateRequiredXp(); // Initialize required XP based on the current level
        UpdateVisuals(); // Update the visuals to reflect the current state

        Candy.onCollected += CandyCollectedCallBack; // Subscribe to the candy collected event
    }

    private void UpdateVisuals()
    {
        m_slider.value = (float)m_currentXp / m_requiredXp; // Update the slider value based on current and required XP
        m_text.text = $"Lvl {m_level}";
    }

    private void UpdateRequiredXp()
    {
        m_requiredXp = m_level * m_levelUpMultiplier; // Update the required XP based on the current level and multiplier
    }

    private void CandyCollectedCallBack(Candy candy)
    {
        m_currentXp++; // Increase current XP by the amount of XP from the collected candy

        if (m_currentXp >= m_requiredXp) // Check if current XP is enough to level up
        {
            LevelUp(); // Call the level up method
        }

        UpdateVisuals(); // Update the visuals after collecting candy
    }

    private void LevelUp()
    {
        m_level++; // Increase the level by 1
        m_currentXp -= m_requiredXp; // Subtract the required XP from current XP
        UpdateRequiredXp(); // Update the required XP for the next level
        UpdateVisuals(); // Update the visuals after leveling up
    }
}
