using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChestObjectContainer : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Image m_icon;
    [SerializeField] private TMP_Text m_nameText;
    [SerializeField] private Image m_outline; // Outline image for the upgrade container
    [SerializeField] private Button m_takeButton; // Button component for the upgrade container
    [SerializeField] private Button m_recycleButton; // Button component for the upgrade container

    [Header("Stats")]
    [SerializeField] private Transform m_statContainersParent;

    [Header("Properties")]
    public Button TakeButton { get { return m_takeButton; } private set { m_takeButton = value; } } // Public property to access the button
    public Button RecycleButton { get { return m_recycleButton; } private set { m_recycleButton = value; } } // Public property to access the button

    [Header("Colors")]
    [SerializeField] private Image[] m_levelDependentImages; // Array to hold level-dependent colors

    public void Configure(ObjectDataSO objectData)
    {
        m_icon.sprite = objectData.IconSprite;
        m_nameText.text = $"{objectData.Name}"; // Set the name text to include the level

        //m_icon.SetNativeSize(); // Set the icon size to its native size
        //
        //m_icon.rectTransform.sizeDelta = m_icon.rectTransform.sizeDelta * (3f / 4f); // Set the icon size to 3/4 of its native size

        Color imageColor = ColorHolder.Instance.GetColor(objectData.Rarity); // Get the color from the ColorHolder singleton
        m_nameText.color = imageColor; // Set the name text color to the level-dependent color

        Color outlineColor = ColorHolder.Instance.GetOutlineColor(objectData.Rarity); // Get the outline color from the ColorHolder singleton
        m_outline.color = outlineColor; // Set the outline color to the level-dependent color

        foreach (Image image in m_levelDependentImages)
        {
            image.color = imageColor; // Set the color of each image in the array to the level-dependent color
        }

        ConfigureStatContainers(objectData.BaseStats); // Configure the stat containers with the weapon data
    }

    private void ConfigureStatContainers(Dictionary<Stat, float> stats)
    {
        StatContainerManager.GenerateStatContainers(stats, m_statContainersParent); // Generate the stat containers using the StatContainerManager
    }
}
