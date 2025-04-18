using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;

public class WaveTransitionManager : MonoBehaviour, IGameStateListener
{
    [Header("Elements")]
    [SerializeField] PlayerStatManager playerStatManager;

    [Header("Settings")]
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

            string buttonString;
            Action action = GetActionToPerform(upgradeType, out buttonString); // Get the action to perform on button click

            m_upgradeContainers[i].Configure(null, upgradeText, buttonString); // Set the text for each upgrade container

            m_upgradeContainers[i].Button.onClick.RemoveAllListeners(); // Remove all listeners from the button
            m_upgradeContainers[i].Button.onClick.AddListener(() => action?.Invoke()); // Add a new listener to handle button clicks
            m_upgradeContainers[i].Button.onClick.AddListener(() => BonusSelectedCallback()); // Add a new listener to handle button clicks
        }
    }

    public void BonusSelectedCallback()
    {
        GameManager.Instance.WaveCompletedCallBack(); // Notify the game manager that the wave is completed
    }

    private Action GetActionToPerform(Stat stat, out string buttonString)
    {
        float value = 0; // Initialize value to zero
        buttonString = ""; // Initialize button string

        switch (stat)
        {
            case Stat.Attack:
                value = UnityEngine.Random.Range(1, 10); // Randomly select a value for the attack upgrade
                buttonString = "+" + value.ToString() + "%"; // Initialize button string
                break;
            case Stat.AttackSpeed:
                value = UnityEngine.Random.Range(1, 10); // Randomly select a value for the attack speed upgrade
                buttonString = "+" + value.ToString() + "%"; // Initialize button string
                break;
            case Stat.CriticalChance:
                value = UnityEngine.Random.Range(1, 10); // Randomly select a value for the critical damage upgrade
                buttonString = "+" + value.ToString() + "%"; // Initialize button string
                break;
            case Stat.CriticalDamage:
                value = UnityEngine.Random.Range(1f, 2f); // Randomly select a value for the move speed upgrade
                buttonString = "+" + value.ToString("F2") + "x"; // Initialize button string
                break;
            case Stat.MoveSpeed:
                value = UnityEngine.Random.Range(1, 10); // Randomly select a value for the attack upgrade
                buttonString = "+" + value.ToString() + "%"; // Initialize button string
                break;
            case Stat.MaxHealth:
                value = UnityEngine.Random.Range(1, 10); // Randomly select a value for the attack speed upgrade
                buttonString = "+" + value.ToString(); // Initialize button string
                break;
            case Stat.Range:
                value = UnityEngine.Random.Range(1f, 5f); // Randomly select a value for the range upgrade
                buttonString = "+" + value.ToString("F2"); // Initialize button string
                break;
            case Stat.HealthRegenation:
                value = UnityEngine.Random.Range(1, 10); // Randomly select a value for the HealthRecoverySpeed upgrade
                buttonString = "+" + value.ToString() + "%"; // Initialize button string
                break;
            case Stat.Armor:
                value = UnityEngine.Random.Range(1, 10); // Randomly select a value for the attack upgrade
                buttonString = "+" + value.ToString() + "%"; // Initialize button string
                break;
            case Stat.Luck:
                value = UnityEngine.Random.Range(1, 10); // Randomly select a value for the attack speed upgrade
                buttonString = "+" + value.ToString() + "%"; // Initialize button string
                break;
            case Stat.Dodge:
                value = UnityEngine.Random.Range(1, 10); // Randomly select a value for the critical damage upgrade
                buttonString = "+" + value.ToString() + "%"; // Initialize button string
                break;
            case Stat.LifeSteal:
                value = UnityEngine.Random.Range(1, 10); // Randomly select a value for the move speed upgrade
                buttonString = "+" + value.ToString() + "%"; // Initialize button string
                break;
            default:
                Debug.LogError($"Stat {stat} not found in the switch statement. "); // Log an error if the stat is not found
                break;
        }

        return () =>
        {
            playerStatManager.AddPlayerStat(stat, value); // Add the selected stat to the player stat manager
        };
    }
}