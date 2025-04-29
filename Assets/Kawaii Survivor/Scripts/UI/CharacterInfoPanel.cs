using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInfoPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text m_nameText;
    [SerializeField] private TMP_Text m_priceText;
    [SerializeField] private GameObject m_priceContainer;
    [SerializeField] private Transform m_statsContainerParent;

    [field: SerializeField] public Button Button { get; private set; }

    public void Configure(CharacterDataSO characterData, bool unlocked)
    {
        m_nameText.text = characterData.Name;
        m_priceText.text = characterData.PurchasePrice.ToString();
        m_priceContainer.SetActive(!unlocked);

        StatContainerManager.GenerateStatContainers(characterData.NonNeutralStats, m_statsContainerParent);
    }
}
