using UnityEngine;
using UnityEngine.UI;
using TMPro; // Assuming you are using TextMeshPro for the text component

public class HealthBar : MonoBehaviour
{
    [Header("Elements")]
    private Slider m_slider;
    private TMP_Text m_text;

    void Awake()
    {
        m_slider = GetComponent<Slider>();
        m_text = GetComponentInChildren<TMP_Text>(); // Assuming the text is a child of the slider
    }

    void Start()
    {
        m_slider.value = 1f; // Set the initial value to 1 (full health)
    }

    public void UpdateHealth(float currentHealth, float maxHealth)
    {
        if (maxHealth > 0)
        {
            m_slider.value = currentHealth / maxHealth; // Update the slider value based on current and max health
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

    public void SetMaxHealth(float maxHealth)
    {
        m_slider.maxValue = maxHealth; // Set the maximum value of the slider
    }

}
