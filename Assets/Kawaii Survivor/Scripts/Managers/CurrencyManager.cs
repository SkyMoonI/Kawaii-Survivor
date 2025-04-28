using System;
using Tabsil.Sijil;
using UnityEngine;

public class CurrencyManager : MonoBehaviour, IWantToBeSaved
{
    public static CurrencyManager Instance { get; private set; } // Singleton instance of the CurrencyManager

    [Header("Settings")]
    [SerializeField] private int m_currency; // Player's currency amount
    public int Currency { get { return m_currency; } private set { m_currency = value; } } // Currency earned per wave
    [SerializeField] private int m_premiumCurrency; // Player's currency amount
    public int PremiumCurrency { get { return m_premiumCurrency; } private set { m_premiumCurrency = value; } } // Currency earned per wave

    private const string PREMIUM_CURRENCY_KEY = "PremiumCurrency";


    [Header("Actions")]
    public static Action onUpdated;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this; // Set the singleton instance
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
        }
    }

    void Start()
    {
        UpdateVisuals(); // Update the currency text at the start
    }

    void OnEnable()
    {
        Candy.onCollected += CandyCollectedCallback;
        Cash.onCollected += CashCollectedCallback;
    }

    void OnDisable()
    {
        Candy.onCollected -= CandyCollectedCallback;
        Cash.onCollected -= CashCollectedCallback;
    }

    void OnDestroy()
    {
        Candy.onCollected -= CandyCollectedCallback;
        Cash.onCollected -= CashCollectedCallback;
    }


    public void AddCurrency(int amount) // Method to add currency
    {
        Currency += amount; // Increase the currency amount
        UpdateVisuals();
    }

    public void AddPremiumCurrency(int amount, bool save = true) // Method to add currency
    {
        PremiumCurrency += amount; // Increase the currency amount
        UpdateVisuals();

        if (save)
        {
            Save();
        }
    }

    public void UseCurrency(int amount) // Method to use currency
    {
        Currency -= amount;
        UpdateVisuals();
    }

    public void UsePremiumCurrency(int amount, bool save = true) // Method to use currency
    {
        PremiumCurrency -= amount;
        UpdateVisuals();

        if (save)
        {
            Save();
        }
    }

    [NaughtyAttributes.Button("Add 500 Currency")]
    private void Add500Currency() // Method to add currency
    {
        AddCurrency(500);
    }

    [NaughtyAttributes.Button("Add 500 Premium Currency")]
    private void Add500PremiumCurrency() // Method to add currency
    {
        AddPremiumCurrency(500);
    }

    private void UpdateVisuals()
    {
        UpdateCurrencyTexts();
        onUpdated?.Invoke();
    }

    private void UpdateCurrencyTexts()
    {
        CurrencyText[] currencyTexts = FindObjectsByType<CurrencyText>(FindObjectsInactive.Include, FindObjectsSortMode.None); // Find all CurrencyText components in the scene

        foreach (CurrencyText currencyText in currencyTexts) // Loop through each CurrencyText component
        {
            currencyText.UpdateCurrencyText(Currency); // Update the currency text with the current currency amount
        }

        PremiumCurrencyText[] premiumCurrencyTexts = FindObjectsByType<PremiumCurrencyText>(FindObjectsInactive.Include, FindObjectsSortMode.None); // Find all CurrencyText components in the scene

        foreach (PremiumCurrencyText premiumCurrencyText in premiumCurrencyTexts) // Loop through each PremiumCurrencyText component
        {
            premiumCurrencyText.UpdateCurrencyText(PremiumCurrency); // Update the currency text with the current currency amount
        }
    }

    public bool HasEnoughCurrency(int amount) // Method to check if the player has enough currency
    {
        return Currency >= amount;
    }

    public bool HasEnoughPremiumCurrency(int amount) // Method to check if the player has enough currency
    {
        return PremiumCurrency >= amount;
    }

    private void CandyCollectedCallback(Candy candy)
    {
        AddCurrency(1);
    }

    private void CashCollectedCallback(Cash cash)
    {
        AddPremiumCurrency(1);
    }

    public void Load()
    {
        if (Sijil.TryLoad(this, PREMIUM_CURRENCY_KEY, out object premiumCurrencyValue))
        {
            AddPremiumCurrency((int)premiumCurrencyValue, false);
        }
        else
        {
            AddPremiumCurrency(0, false);
        }
    }

    public void Save()
    {
        Sijil.Save(this, PREMIUM_CURRENCY_KEY, PremiumCurrency);
    }
}
