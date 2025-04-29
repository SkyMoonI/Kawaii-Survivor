using System;
using System.Collections.Generic;
using Tabsil.Sijil;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectionManager : MonoBehaviour, IWantToBeSaved
{
    [Header("Elements")]
    [SerializeField] private Transform m_characterButtonsParent;
    [SerializeField] private CharacterSelectionButton m_characterButtonPrefab;
    [SerializeField] private Image m_characterImage;
    [SerializeField] private CharacterInfoPanel m_characterInfoPanel;

    [Header("Datum")]
    private CharacterDataSO[] characterDatum;
    private List<bool> m_unlockedStates = new List<bool>();
    private const string UNLOCKED_STATES_KEY = "UNLOCKED_STATES_KEY";
    private const string LAST_SELECTED_CHARACTER_INDEX_KEY = "LAST_SELECTED_CHARACTER_INDEX_KEY";

    [Header("Settings")]
    private int m_lastSelectedCharacterIndex;
    private int m_selectedCharacterIndex;

    public static Action<CharacterDataSO> onCharacterSelected;

    void Start()
    {
        Initialize();

        CharacterSelectedCallback(m_lastSelectedCharacterIndex);
    }

    void OnEnable()
    {
        m_characterInfoPanel.Button.onClick.RemoveAllListeners();
        m_characterInfoPanel.Button.onClick.AddListener(PurchaseSelectedCallback);
    }

    void OnDisable()
    {
        m_characterInfoPanel.Button.onClick.RemoveAllListeners();
    }

    void OnDestroy()
    {
        m_characterInfoPanel.Button.onClick.RemoveAllListeners();
    }

    private void Initialize()
    {
        for (int i = 0; i < characterDatum.Length; i++)
        {
            CreateCharacterButton(i);
        }

    }

    private void CreateCharacterButton(int index)
    {
        CharacterDataSO characterData = characterDatum[index];
        CharacterSelectionButton characterButtonInstance = Instantiate(m_characterButtonPrefab, m_characterButtonsParent);
        characterButtonInstance.Configure(characterData.Sprite, m_unlockedStates[index]);

        characterButtonInstance.Button.onClick.RemoveAllListeners();
        characterButtonInstance.Button.onClick.AddListener(() => CharacterSelectedCallback(index));
    }

    private void CharacterSelectedCallback(int index)
    {
        m_selectedCharacterIndex = index;

        CharacterDataSO characterData = characterDatum[index];
        m_characterImage.sprite = characterData.Sprite;

        if (!m_unlockedStates[index])
        {
            m_characterInfoPanel.Button.interactable = CurrencyManager.Instance.HasEnoughPremiumCurrency(characterData.PurchasePrice);
        }
        else
        {
            m_lastSelectedCharacterIndex = index;
            Save();

            onCharacterSelected?.Invoke(characterData);
        }

        m_characterInfoPanel.Configure(characterData, m_unlockedStates[index]);
    }

    private void PurchaseSelectedCallback()
    {
        int price = characterDatum[m_selectedCharacterIndex].PurchasePrice;
        CurrencyManager.Instance.UsePremiumCurrency(price);

        m_unlockedStates[m_selectedCharacterIndex] = true;

        m_characterButtonsParent.GetChild(m_selectedCharacterIndex).GetComponent<CharacterSelectionButton>().UnLock();

        CharacterSelectedCallback(m_selectedCharacterIndex);

        Save();
    }

    public void Load()
    {
        characterDatum = ResourcesManager.Characters;

        for (int i = 0; i < characterDatum.Length; i++)
        {
            m_unlockedStates.Add(i == 0);
        }

        Sijil.TryLoad(this, UNLOCKED_STATES_KEY, out object unlockedStates);

        if (unlockedStates != null)
        {
            m_unlockedStates = (List<bool>)unlockedStates;
        }

        Sijil.TryLoad(this, LAST_SELECTED_CHARACTER_INDEX_KEY, out object lastSelectedCharacterIndex);

        if (lastSelectedCharacterIndex != null)
        {
            m_lastSelectedCharacterIndex = (int)lastSelectedCharacterIndex;
        }
    }

    public void Save()
    {
        Sijil.Save(this, UNLOCKED_STATES_KEY, m_unlockedStates);
        Sijil.Save(this, LAST_SELECTED_CHARACTER_INDEX_KEY, m_lastSelectedCharacterIndex);
    }
}
