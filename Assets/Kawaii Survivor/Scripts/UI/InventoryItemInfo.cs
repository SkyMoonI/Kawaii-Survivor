using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemInfo : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Image m_itemIcon;
    [SerializeField] private TMP_Text m_itemNameText;
    [SerializeField] private TMP_Text m_itemRecyclePriceText;

    [Header("Colors")]
    [SerializeField] private Image m_containerImage;

    [Header("Stats")]
    [SerializeField] private Transform m_statContainersParent;

    [Header("Buttons")]
    [field: SerializeField] public Button RecycleButton { get; private set; }
    [SerializeField] private Button m_mergeButton;

    private void Configure(Sprite itemIcon, Color containerColor, string itemName, int recyclePrice, Dictionary<Stat, float> stats)
    {
        m_itemIcon.sprite = itemIcon;
        m_itemNameText.text = itemName;
        m_itemNameText.color = containerColor;
        m_itemRecyclePriceText.text = recyclePrice.ToString();
        m_containerImage.color = containerColor;

        StatContainerManager.GenerateStatContainers(stats, m_statContainersParent);
    }

    public void Configure(ObjectDataSO objectData)
    {
        Configure(objectData.IconSprite,
         ColorHolder.Instance.GetColor(objectData.Rarity),
          objectData.Name,
           objectData.RecyclePrice,
            objectData.BaseStats);

        m_mergeButton.gameObject.SetActive(false);
    }

    public void Configure(Weapon weapon)
    {
        Configure(weapon.WeaponData.IconSprite,
         ColorHolder.Instance.GetColor(weapon.Level),
          weapon.WeaponData.Name + " (lvl " + (weapon.Level + 1) + ")",
           WeaponStatsCalculator.GetRecyclePrice(weapon.WeaponData, weapon.Level),
            WeaponStatsCalculator.GetStats(weapon.WeaponData, weapon.Level));


        m_mergeButton.gameObject.SetActive(true);

        m_mergeButton.interactable = WeaponMerger.Instance.CanMerge(weapon);

        m_mergeButton.onClick.RemoveAllListeners();
        m_mergeButton.onClick.AddListener(WeaponMerger.Instance.Merge);
    }


}
