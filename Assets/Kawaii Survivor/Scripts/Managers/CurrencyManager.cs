using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance { get; private set; } // Singleton instance of the CurrencyManager

    [Header("Settings")]
    [SerializeField] private int m_currency; // Player's currency amount
    public int Currency { get { return m_currency; } private set { m_currency = value; } } // Currency earned per wave

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
        UpdateCurrencyTexts(); // Update the currency text at the start
    }

    public void AddCurrency(int amount) // Method to add currency
    {
        Currency += amount; // Increase the currency amount
        UpdateCurrencyTexts();
    }

    private void UpdateCurrencyTexts()
    {
        CurrencyText[] currencyTexts = FindObjectsByType<CurrencyText>(FindObjectsInactive.Include, FindObjectsSortMode.None); // Find all CurrencyText components in the scene

        foreach (CurrencyText currencyText in currencyTexts) // Loop through each CurrencyText component
        {
            currencyText.UpdateCurrencyText(Currency); // Update the currency text with the current currency amount
        }
    }
}
