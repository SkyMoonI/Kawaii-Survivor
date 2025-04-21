using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatContainer : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Image m_icon;
    [SerializeField] private TMP_Text m_nameText;
    [SerializeField] private TMP_Text m_valueText;

    public void Configure(Sprite icon, string name, string value)
    {
        m_icon.sprite = icon;
        m_nameText.text = name;
        m_valueText.text = value;
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
