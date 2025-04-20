using UnityEngine;
using UnityEngine.UI;
using TMPro; // Assuming you are using TextMeshPro for the text component

public class HealthBar : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Slider m_slider;
    [SerializeField] private TMP_Text m_text;

    void Start()
    {
        m_slider.value = 1f; // Set the initial value to 1 (full health)
    }

    public void UpdateHealth(float currentHealth, float maxHealth)
    {
        if (maxHealth > 0)
        {
            m_slider.value = (float)currentHealth / maxHealth; // Update the slider value based on current and max health
            UpdateText(currentHealth, maxHealth);
        }
    }

    private void UpdateText(float currentHealth, float maxHealth)
    {
        if (m_text != null)
        {
            m_text.text = $"{currentHealth} / {maxHealth}"; // Update the text to show current and max health
        }
    }

}
