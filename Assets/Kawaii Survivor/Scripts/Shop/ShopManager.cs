using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour, IGameStateListener
{
    [Header("Elemenets")]
    [SerializeField] private Transform shopItemContainerParent;
    [SerializeField] private ShopItemContainer shopItemContainerPrefab;
    [SerializeField] private Button m_rerollButton;

    [Header("Settings")]
    [SerializeField] private int m_rerollCost;
    [SerializeField] private TMP_Text m_rerollCostText;

    [Header("Player")]
    [SerializeField] private PlayerWeapons m_playerWeapons;
    [SerializeField] private PlayerObjects m_playerObjects;


    void OnEnable()
    {
        m_rerollButton.onClick.AddListener(Reroll);
        CurrencyManager.onUpdated += CurrencyUpdatedCallback;
        ShopItemContainer.onPurchased += ItemPurchasedCallback;
    }

    void OnDisable()
    {
        m_rerollButton.onClick.RemoveListener(Reroll);
        CurrencyManager.onUpdated -= CurrencyUpdatedCallback;
        ShopItemContainer.onPurchased -= ItemPurchasedCallback;
    }

    void OnDestroy()
    {
        m_rerollButton.onClick.RemoveListener(Reroll);
        CurrencyManager.onUpdated -= CurrencyUpdatedCallback;
        ShopItemContainer.onPurchased -= ItemPurchasedCallback;
    }

    public void GameStateChangedCallBack(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.SHOP:
                Configure();
                UpdateRerollVisual();
                break;
        }
    }

    private void Configure()
    {
        List<ShopItemContainer> shopItemContainersToDestroy = new List<ShopItemContainer>();

        for (int i = 0; i < shopItemContainerParent.childCount; i++)
        {
            ShopItemContainer shopItemContainer = shopItemContainerParent.GetChild(i).GetComponent<ShopItemContainer>();
            if (!shopItemContainer.IsLocked)
            {
                shopItemContainersToDestroy.Add(shopItemContainer);
            }
        }

        while (shopItemContainersToDestroy.Count > 0)
        {
            Transform transform = shopItemContainersToDestroy[0].transform;
            transform.SetParent(null);
            Destroy(transform.gameObject);
            shopItemContainersToDestroy.RemoveAt(0); // Remove the item from the list after destroying it
        }

        int containersToAdd = 6 - shopItemContainerParent.childCount; // Calculate the number of containers to add 
        int weaponContainerCount = UnityEngine.Random.Range(Mathf.Min(2, containersToAdd), containersToAdd);
        int objectContainerCount = containersToAdd - weaponContainerCount;

        for (int i = 0; i < weaponContainerCount; i++)
        {
            ShopItemContainer shopItemContainerInstance = Instantiate(shopItemContainerPrefab, shopItemContainerParent);

            WeaponDataSO randomWeapon = ResourcesManager.GetRandomWeapon(); // Get a random object from the ResourcesManager

            shopItemContainerInstance.Configure(randomWeapon, UnityEngine.Random.Range(0, 4)); // Configure the shop item container with the random object
        }
        for (int i = 0; i < objectContainerCount; i++)
        {
            ShopItemContainer shopItemContainerInstance = Instantiate(shopItemContainerPrefab, shopItemContainerParent);

            ObjectDataSO randomObject = ResourcesManager.GetRandomObject(); // Get a random object from the ResourcesManager

            shopItemContainerInstance.Configure(randomObject); // Configure the shop item container with the random object
        }
    }

    private void Reroll()
    {
        Configure();
        CurrencyManager.Instance.UseCurrency(m_rerollCost);
    }

    private void UpdateRerollVisual()
    {
        m_rerollCostText.text = m_rerollCost.ToString();

        m_rerollButton.interactable = CurrencyManager.Instance.HasEnoughCurrency(m_rerollCost); // Check if the player has enough currency to reroll
    }

    private void CurrencyUpdatedCallback()
    {
        UpdateRerollVisual();
    }

    private void ItemPurchasedCallback(ShopItemContainer shopItemContainer, int weaponLevel)
    {
        if (shopItemContainer.WeaponData != null)
        {
            TryPurchaseWeapon(shopItemContainer, weaponLevel);
        }
        else
        {
            PurchaseObject(shopItemContainer);
        }
    }

    private void TryPurchaseWeapon(ShopItemContainer shopItemContainer, int weaponLevel)
    {
        if (m_playerWeapons.TryAddWeapon(shopItemContainer.WeaponData, weaponLevel))
        {
            int purchasePrice = WeaponStatsCalculator.GetPurchasePrice(shopItemContainer.WeaponData, weaponLevel);
            CurrencyManager.Instance.UseCurrency(purchasePrice);

            Destroy(shopItemContainer.gameObject);
        }
    }

    private void PurchaseObject(ShopItemContainer shopItemContainer)
    {
        m_playerObjects.AddObject(shopItemContainer.ObjectData);

        int purchasePrice = shopItemContainer.ObjectData.PurchasePrice;
        CurrencyManager.Instance.UseCurrency(purchasePrice);

        Destroy(shopItemContainer.gameObject);
    }
}
