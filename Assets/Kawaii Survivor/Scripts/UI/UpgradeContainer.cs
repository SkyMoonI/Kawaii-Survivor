using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeContainer : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Image m_icon; // Icon for the upgrade container
    [SerializeField] private TMP_Text m_upgradeNameText; // Text for the upgrade container
    [SerializeField] private TMP_Text m_upgradeValueText; // Description text for the upgrade container
    [SerializeField] private Button m_button; // Button component for the upgrade container

    [Header("Properties")]
    public Button Button { get { return m_button; } private set { m_button = value; } } // Public property to access the button

    public void Configure(Sprite icon, string upgradeName, string upgradeValue)
    {
        m_icon.sprite = icon;
        m_upgradeNameText.text = upgradeName;
        m_upgradeValueText.text = upgradeValue;
    }
}
