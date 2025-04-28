using System;
using UnityEngine;

public class InventoryManager : MonoBehaviour, IGameStateListener
{
    [Header("Player Elements")]
    [SerializeField] private PlayerObjects m_playerObjects;
    [SerializeField] private PlayerWeapons m_playerWeapons;

    [Header("Elements")]
    [SerializeField] private Transform m_inventoryItemsParent;
    [SerializeField] private Transform m_pauseInventoryItemsParent;
    [SerializeField] private InventoryItemContainer m_inventoryItemContainer;
    [SerializeField] private ShopManagerUI m_shopManagerUI;
    [SerializeField] private InventoryItemInfo m_inventoryItemInfo;

    void OnEnable()
    {
        ShopManager.onItemPurchased += ItemPurchasedCallback;
        WeaponMerger.onMerge += WeaponMergedCallback;

        GameManager.onGamePaused += Configure;
    }

    void OnDisable()
    {
        ShopManager.onItemPurchased -= ItemPurchasedCallback;
        WeaponMerger.onMerge -= WeaponMergedCallback;

        GameManager.onGamePaused -= Configure;
    }

    void OnDestroy()
    {
        ShopManager.onItemPurchased -= ItemPurchasedCallback;
        WeaponMerger.onMerge -= WeaponMergedCallback;

        GameManager.onGamePaused -= Configure;
    }


    public void GameStateChangedCallBack(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.SHOP:
                Configure();
                break;
        }
    }

    private void Configure()
    {
        m_inventoryItemsParent.Clear();
        m_pauseInventoryItemsParent.Clear();

        Weapon[] weapons = m_playerWeapons.GetWeapons(); // Get all weapons from the player weapons

        for (int i = 0; i < weapons.Length; i++)
        {
            // If the weapon is null, continue so that the null weapons are not added with the index
            if (weapons[i] == null)
            {
                continue;
            }

            InventoryItemContainer inventoryItemContainerInstance = Instantiate(m_inventoryItemContainer, m_inventoryItemsParent);
            inventoryItemContainerInstance.Configure(weapons[i], i, () => ShowItemInfo(inventoryItemContainerInstance));

            InventoryItemContainer pauseInventoryItemContainerInstance = Instantiate(m_inventoryItemContainer, m_pauseInventoryItemsParent);
            pauseInventoryItemContainerInstance.Configure(weapons[i], i, null);
        }

        ObjectDataSO[] objectDatas = m_playerObjects.Objects.ToArray(); // Get all object data from the player objects

        for (int i = 0; i < objectDatas.Length; i++)
        {
            InventoryItemContainer inventoryItemContainerInstance = Instantiate(m_inventoryItemContainer, m_inventoryItemsParent);
            inventoryItemContainerInstance.Configure(objectDatas[i], () => ShowItemInfo(inventoryItemContainerInstance));

            InventoryItemContainer pauseInventoryItemContainerInstance = Instantiate(m_inventoryItemContainer, m_pauseInventoryItemsParent);
            pauseInventoryItemContainerInstance.Configure(objectDatas[i], null);
        }
    }

    private void ShowItemInfo(InventoryItemContainer container)
    {
        if (container.WeaponData != null)
        {
            ShowWeaponInfo(container.WeaponData, container.WeaponIndex);
        }
        else
        {
            ShowObjectInfo(container.ObjectData);
        }
    }

    private void ShowWeaponInfo(Weapon weapon, int index)
    {
        m_inventoryItemInfo.Configure(weapon);

        m_inventoryItemInfo.RecycleButton.onClick.RemoveAllListeners();
        m_inventoryItemInfo.RecycleButton.onClick.AddListener(() => RecycleWeapon(index));

        m_shopManagerUI.ShowItemInfo();
    }

    private void RecycleWeapon(int index)
    {
        m_playerWeapons.RecycleWeapon(index); // Recycle the weapon from the player weapons

        Configure(); // Refresh the inventory

        m_shopManagerUI.HideItemInfo(); // Hide the item info because the weapon has been recycled
    }

    private void ShowObjectInfo(ObjectDataSO objectData)
    {
        m_inventoryItemInfo.Configure(objectData);

        m_inventoryItemInfo.RecycleButton.onClick.RemoveAllListeners();
        m_inventoryItemInfo.RecycleButton.onClick.AddListener(() => RecycleObject(objectData));

        m_shopManagerUI.ShowItemInfo();
    }

    private void RecycleObject(ObjectDataSO objectData)
    {
        m_playerObjects.RecycleObject(objectData); // Recycle the object from the player objects

        Configure(); // Refresh the inventory

        m_shopManagerUI.HideItemInfo(); // Hide the item info because the object has been recycled
    }

    private void ItemPurchasedCallback()
    {
        Configure();
    }

    private void WeaponMergedCallback(Weapon weapon)
    {
        Configure();

        m_inventoryItemInfo.Configure(weapon);
    }
}
