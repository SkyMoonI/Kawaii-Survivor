using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;

public class WaveTransitionManager : MonoBehaviour, IGameStateListener
{
    [SerializeField] private UpgradeContainer[] m_upgradeContainers; // Array of buttons for upgrades

    public void GameStateChangedCallBack(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.WAVETRANSITION:
                ConfigureUpgradeContainers(); // Show upgrade containers when transitioning to the wave
                break;
        }
    }

    [Button()]
    private void ConfigureUpgradeContainers()
    {
        for (int i = 0; i < m_upgradeContainers.Length; i++)
        {
            int randomUpgradeIndex = UnityEngine.Random.Range(0, Enum.GetValues(typeof(Stat)).Length); // Randomly select an upgrade type
            Stat upgradeType = (Stat)randomUpgradeIndex; // Cast the random index to Stat enum
            string upgradeText = Enums.FormatStatName(upgradeType); // Create the upgrade text based on the selected type

            string randomValue = UnityEngine.Random.Range(1, 10).ToString(); // Randomly select a value for the upgrade

            m_upgradeContainers[i].Configure(null, upgradeText, randomValue); // Set the text for each upgrade container

            m_upgradeContainers[i].Button.onClick.RemoveAllListeners(); // Remove all listeners from the button
            m_upgradeContainers[i].Button.onClick.AddListener(() => BonusSelectedCallback()); // Add a new listener to handle button clicks
        }
    }

    public void BonusSelectedCallback()
    {
        GameManager.Instance.WaveCompletedCallBack(); // Notify the game manager that the wave is completed
    }
}

