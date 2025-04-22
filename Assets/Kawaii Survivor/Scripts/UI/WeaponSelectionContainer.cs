using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSelectionContainer : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Image m_icon;
    [SerializeField] private TMP_Text m_nameText;
    [SerializeField] private Button m_button; // Button component for the upgrade container
    [SerializeField] private Image m_outline; // Outline image for the upgrade container

    [Header("Settings")]
    [SerializeField] private float m_scaleFactor = 1.075f; // Size of the icon
    [SerializeField] private float m_scaleTime = 0.3f; // Size of the icon

    [Header("Stats")]
    [SerializeField] private Transform m_statContainersParent;

    [Header("Properties")]
    public Button Button { get { return m_button; } private set { m_button = value; } } // Public property to access the button

    [Header("Colors")]
    [SerializeField] private Image[] m_levelDependentImages; // Array to hold level-dependent colors

    public void Configure(WeaponDataSO weaponData, int level)
    {
        m_icon.sprite = weaponData.IconSprite;
        m_nameText.text = $"{weaponData.Name}\n(lvl {level + 1})"; // Set the name text to include the level

        Color imageColor = ColorHolder.Instance.GetColor(level); // Get the color from the ColorHolder singleton
        m_nameText.color = imageColor; // Set the name text color to the level-dependent color

        Color outlineColor = ColorHolder.Instance.GetOutlineColor(level); // Get the outline color from the ColorHolder singleton
        m_outline.color = outlineColor; // Set the outline color to the level-dependent color

        foreach (Image image in m_levelDependentImages)
        {
            image.color = imageColor; // Set the color of each image in the array to the level-dependent color
        }

        Dictionary<Stat, float> calculatedBaseStats = WeaponStatsCalculator.GetStats(weaponData, level); // Get the stats for the weapon data and level
        ConfigureStatContainers(calculatedBaseStats); // Configure the stat containers with the weapon data
    }

    private void ConfigureStatContainers(Dictionary<Stat, float> calculatedBaseStats)
    {
        StatContainerManager.GenerateStatContainers(calculatedBaseStats, m_statContainersParent); // Generate the stat containers using the StatContainerManager
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
