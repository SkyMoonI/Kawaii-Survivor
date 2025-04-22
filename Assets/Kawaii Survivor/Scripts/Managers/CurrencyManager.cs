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

    public void AddCurrency(int amount) // Method to add currency
    {
        Currency += amount; // Increase the currency amount
    }
}
