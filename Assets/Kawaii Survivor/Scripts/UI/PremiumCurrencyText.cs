using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))] // Require a TextMeshProUGUI component
public class PremiumCurrencyText : MonoBehaviour
{
    private TMP_Text currencyText;

    public void UpdateCurrencyText(int currencyAmount)
    {
        if (currencyText == null)
        {
            currencyText = GetComponent<TMP_Text>(); // Get the TextMeshProUGUI component
        }

        currencyText.text = currencyAmount.ToString("N0"); // Format the currency amount with thousands separator
    }
}
