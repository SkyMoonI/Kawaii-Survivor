using System;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemContainer : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Image m_iconImage;
    [SerializeField] private Image m_containerImage;
    [SerializeField] private Button m_clickButton;

    [Header("Settings")]
    public Weapon WeaponData { get; private set; }
    public ObjectDataSO ObjectData { get; private set; }
    public int WeaponIndex { get; private set; }

    private void Configure(Sprite itemIcon, Color containerCOlor)
    {
        m_iconImage.sprite = itemIcon;
        m_containerImage.color = containerCOlor;
    }

    public void Configure(Weapon weapon, int index, Action clickedCallback)
    {
        WeaponData = weapon;
        WeaponIndex = index;

        Sprite iconSprite = weapon.WeaponData.IconSprite; // Get the icon sprite from the resources manager
        Color containerColor = ColorHolder.Instance.GetColor(weapon.Level);

        Configure(iconSprite, containerColor);
        m_clickButton.onClick.RemoveAllListeners();
        m_clickButton.onClick.AddListener(() => clickedCallback?.Invoke());
    }
    public void Configure(ObjectDataSO objectData, Action clickedCallback)
    {
        ObjectData = objectData;

        Sprite iconSprite = objectData.IconSprite; // Get the icon sprite from the resources manager
        Color containerColor = ColorHolder.Instance.GetColor(objectData.Rarity);

        Configure(iconSprite, containerColor);
        m_clickButton.onClick.RemoveAllListeners();
        m_clickButton.onClick.AddListener(() => clickedCallback?.Invoke());
    }

}
