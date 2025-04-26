using UnityEngine;

public class InventoryManager : MonoBehaviour, IGameStateListener
{
    [Header("Elements")]
    [SerializeField] private Transform m_inventoryItemsParent;
    [SerializeField] private InventoryItemContainer m_inventoryItemContainer;
    [SerializeField] private PlayerObjects m_playerObjects;
    [SerializeField] private PlayerWeapons m_playerWeapons;
    [SerializeField] private ShopManagerUI m_shopManagerUI;
    [SerializeField] private InventoryItemInfo m_inventoryItemInfo;

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

        Weapon[] weapons = m_playerWeapons.GetWeapons(); // Get all weapons from the player weapons

        for (int i = 0; i < weapons.Length; i++)
        {
            InventoryItemContainer inventoryItemContainerInstance = Instantiate(m_inventoryItemContainer, m_inventoryItemsParent);

            inventoryItemContainerInstance.Configure(weapons[i], () => ShowItemInfo(inventoryItemContainerInstance));
        }

        ObjectDataSO[] objectDatas = m_playerObjects.Objects.ToArray(); // Get all object data from the player objects

        for (int i = 0; i < objectDatas.Length; i++)
        {
            InventoryItemContainer inventoryItemContainerInstance = Instantiate(m_inventoryItemContainer, m_inventoryItemsParent);

            inventoryItemContainerInstance.Configure(objectDatas[i], () => ShowItemInfo(inventoryItemContainerInstance));
        }
    }

    private void ShowItemInfo(InventoryItemContainer container)
    {
        if (container.WeaponData != null)
        {
            ShowWeaponInfo(container.WeaponData);
        }
        else
        {
            ShowObjectInfo(container.ObjectData);
        }
    }

    private void ShowWeaponInfo(Weapon weapon)
    {
        m_inventoryItemInfo.Configure(weapon);

        m_shopManagerUI.ShowItemInfo();
    }

    private void ShowObjectInfo(ObjectDataSO objectData)
    {
        m_inventoryItemInfo.Configure(objectData);

        m_shopManagerUI.ShowItemInfo();
    }
}
