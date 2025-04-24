using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using System;

public class ShopItemContainer : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Image m_icon;
    [SerializeField] private TMP_Text m_nameText;
    [SerializeField] private TMP_Text m_priceText;
    [SerializeField] private Button m_purchaseButton; // Button component for the upgrade container
    [SerializeField] private Image m_outline; // Outline image for the upgrade container

    [Header("Stats")]
    [SerializeField] private Transform m_statContainersParent;

    [Header("Lock Settings")]
    [SerializeField] private Button m_lockButton; // Button component for the lock button
    [SerializeField] private Sprite m_lockSprite, m_unlockSprite; // Image component for the lock button
    public bool IsLocked { get; private set; } = false; // Property to check if the item is locked

    [Header("Colors")]
    [SerializeField] private Image[] m_levelDependentImages; // Array to hold level-dependent colors

    [Header("Actions")]
    public static Action<ShopItemContainer, int> onPurchased;

    [Header("Purchase Settings")]
    public WeaponDataSO WeaponData { get; private set; }
    public ObjectDataSO ObjectData { get; private set; }
    private int m_weaponLevel;

    void OnEnable()
    {
        CurrencyManager.onUpdated += CurrencuUpdatedCallback;
    }

    void OnDisable()
    {
        CurrencyManager.onUpdated -= CurrencuUpdatedCallback;
        m_purchaseButton.onClick.RemoveAllListeners();
    }

    void OnDestroy()
    {
        CurrencyManager.onUpdated -= CurrencuUpdatedCallback;
        m_purchaseButton.onClick.RemoveAllListeners();
    }

    public void Configure(WeaponDataSO weaponData, int level)
    {
        WeaponData = weaponData;

        m_weaponLevel = level;

        m_icon.sprite = weaponData.IconSprite;
        m_nameText.text = $"{weaponData.Name}\n(lvl {level + 1})"; // Set the name text to include the level

        int weaponPrice = WeaponStatsCalculator.GetPurchasePrice(weaponData, level);
        m_priceText.text = weaponPrice.ToString(); // Set the price text to the weapon data price

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

        UpdateLockVisual(); // Update the lock visual based on the lock state
        m_lockButton.onClick.AddListener(() => LockButtonCallback()); // Add a listener to the lock button to toggle the lock state

        m_purchaseButton.interactable = CurrencyManager.Instance.HasEnoughCurrency(weaponPrice);
        m_purchaseButton.onClick.AddListener(Purchase);
    }

    private void ConfigureStatContainers(Dictionary<Stat, float> stats)
    {
        m_statContainersParent.Clear(); // Destroy all the children of the stat containers parent container
        StatContainerManager.GenerateStatContainers(stats, m_statContainersParent); // Generate the stat containers using the StatContainerManager
    }

    public void Configure(ObjectDataSO objectData)
    {
        ObjectData = objectData;

        m_icon.sprite = objectData.IconSprite;
        m_nameText.text = $"{objectData.Name})"; // Set the name text to include the level
        m_priceText.text = objectData.PurchasePrice.ToString(); // Set the price text to the weapon data price

        Color imageColor = ColorHolder.Instance.GetColor(objectData.Rarity); // Get the color from the ColorHolder singleton
        m_nameText.color = imageColor; // Set the name text color to the level-dependent color

        Color outlineColor = ColorHolder.Instance.GetOutlineColor(objectData.Rarity); // Get the outline color from the ColorHolder singleton
        m_outline.color = outlineColor; // Set the outline color to the level-dependent color

        foreach (Image image in m_levelDependentImages)
        {
            image.color = imageColor; // Set the color of each image in the array to the level-dependent color
        }

        ConfigureStatContainers(objectData.BaseStats); // Configure the stat containers with the weapon data

        UpdateLockVisual(); // Update the lock visual based on the lock state
        m_lockButton.onClick.AddListener(() => LockButtonCallback()); // Add a listener to the lock button to toggle the lock state

        m_purchaseButton.interactable = CurrencyManager.Instance.HasEnoughCurrency(objectData.PurchasePrice);
        m_purchaseButton.onClick.AddListener(Purchase);
    }

    private void LockButtonCallback()
    {
        IsLocked = !IsLocked; // Toggle the lock state
        UpdateLockVisual();
    }

    private void UpdateLockVisual()
    {
        m_lockButton.GetComponent<Image>().sprite = IsLocked ? m_lockSprite : m_unlockSprite; // Set the lock button sprite based on the lock state
    }

    private void Purchase()
    {
        onPurchased?.Invoke(this, m_weaponLevel);
    }

    private void CurrencuUpdatedCallback()
    {
        int itemPrice;

        if (WeaponData != null)
        {
            itemPrice = WeaponStatsCalculator.GetPurchasePrice(WeaponData, m_weaponLevel);
        }
        else
        {
            itemPrice = ObjectData.PurchasePrice;
        }

        m_purchaseButton.interactable = CurrencyManager.Instance.HasEnoughCurrency(itemPrice);
    }
}
