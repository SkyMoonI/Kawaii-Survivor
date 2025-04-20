using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSelectionContainer : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Image m_icon;
    [SerializeField] private TMP_Text m_nameText;
    [SerializeField] private Button m_button; // Button component for the upgrade container

    [Header("Settings")]
    [SerializeField] private float m_scaleFactor = 1.075f; // Size of the icon
    [SerializeField] private float m_scaleTime = 0.3f; // Size of the icon

    [Header("Properties")]
    public Button Button { get { return m_button; } private set { m_button = value; } } // Public property to access the button

    [Header("Colors")]
    [SerializeField] private Image[] m_levelDependentImages; // Array to hold level-dependent colors

    public void Configure(Sprite icon, string name, int level)
    {
        m_icon.sprite = icon;
        m_nameText.text = name;

        m_icon.SetNativeSize(); // Set the icon size to its native size

        m_icon.rectTransform.sizeDelta = m_icon.rectTransform.sizeDelta * (3f / 4f); // Set the icon size to 3/4 of its native size

        Color imageColor = ColorHolder.Instance.GetColor(level); // Get the color from the ColorHolder singleton

        foreach (Image image in m_levelDependentImages)
        {
            image.color = imageColor; // Set the color of each image in the array to the level-dependent color
        }
    }

    public void Select()
    {
        LeanTween.cancel(gameObject); // Cancel any previous animations on the icon
        LeanTween.scale(gameObject, Vector3.one * m_scaleFactor, m_scaleTime).setEaseOutBack(); // Scale the icon to 1.2 times its size
    }

    public void Deselect()
    {
        LeanTween.cancel(gameObject); // Cancel any previous animations on the icon
        LeanTween.scale(gameObject, Vector3.one, m_scaleTime).setEaseOutBack(); // Scale the icon to 1.2 times its size
    }

}
