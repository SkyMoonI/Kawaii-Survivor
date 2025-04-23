using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatContainer : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Image m_icon;
    [SerializeField] private TMP_Text m_nameText;
    [SerializeField] private TMP_Text m_valueText;

    public void Configure(Sprite icon, string name, float value, bool useColor = false)
    {
        if (useColor)
        {
            m_valueText.color = GetColorFromValue(value); // Set the color of the name text to red if useColor is true
        }
        else
        {
            m_valueText.color = Color.white; // Set the color of the name text to white if useColor is false
        }

        m_icon.sprite = icon;
        m_nameText.text = name;
        m_valueText.text = value.ToString("F2"); // Format the value to one decimal place
    }

    private Color GetColorFromValue(float value)
    {
        if (value > 0)
        {
            return Color.green; // Return green if the value is positive
        }
        else if (value < 0)
        {
            return Color.red; // Return red if the value is negative
        }
        else
        {
            return Color.white; // Return white if the value is zero    
        }
    }
    public float GetFontSize()
    {
        return m_nameText.fontSize; // Return the font size of the name text
    }
    public void SetFontSize(float size)
    {
        m_nameText.fontSizeMax = size; // Set the font size of the name text
        m_valueText.fontSizeMax = size; // Set the font size of the value text
    }
}
