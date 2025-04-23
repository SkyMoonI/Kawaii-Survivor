using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;

public class WaveTransitionManager : MonoBehaviour, IGameStateListener
{
    public static WaveTransitionManager Instance { get; private set; } // Singleton instance of the WaveTransitionManager

    [Header("Elements")]
    [SerializeField] private PlayerObjects m_playerObjects; // Reference to the player objects

    [Header("Settings")]
    [SerializeField] private UpgradeContainer[] m_upgradeContainers; // Array of buttons for upgrades
    [SerializeField] private Transform m_upgradeContainersParent; // Parent transform for the chest object container
    private int m_chestCollectedCount; // Number of chests collected

    [Header("Chest Container Settings")]
    [SerializeField] private ChestObjectContainer m_chestObjectContainerPrefab; // Reference to the chest object
    [SerializeField] private Transform m_chestContainersParent; // Parent transform for the chest object container

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
    void OnEnable()
    {
        Chest.onCollected += ChestCollectedCallBack; // Subscribe to the chest collected event
    }
    void OnDisable()
    {
        Chest.onCollected -= ChestCollectedCallBack; // Unsubscribe from the chest collected event
    }
    void OnDestroy()
    {
        Chest.onCollected -= ChestCollectedCallBack; // Unsubscribe from the chest collected event
    }

    public void GameStateChangedCallBack(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.WAVETRANSITION:
                TryOpenChest();
                break;
        }
    }

    private void TryOpenChest()
    {
        m_chestContainersParent.Clear();

        if (m_chestCollectedCount > 0)
        {
            ShowObject();
        }
        else
        {
            ConfigureUpgradeContainers(); // Show upgrade containers when transitioning to the wave
        }
    }

    private void ShowObject()
    {
        m_upgradeContainersParent.gameObject.SetActive(false);

        m_chestCollectedCount--;

        ObjectDataSO[] objectDatas = ResourcesManager.Objects; // Get all object data from the resources manager
        ObjectDataSO randomObjectData = objectDatas[UnityEngine.Random.Range(0, objectDatas.Length)]; // Randomly select an object data

        ChestObjectContainer chestObjectContainer = Instantiate(m_chestObjectContainerPrefab, m_chestContainersParent); // Instantiate the chest object container prefab
        chestObjectContainer.Configure(randomObjectData); // Configure the chest object container with the random object data

        chestObjectContainer.TakeButton.onClick.RemoveAllListeners(); // Remove all listeners from the button
        chestObjectContainer.TakeButton.onClick.AddListener(() => TakeButtonCallback(randomObjectData)); // Add a new listener to handle button clicks

        chestObjectContainer.RecycleButton.onClick.RemoveAllListeners(); // Remove all listeners from the button
        chestObjectContainer.RecycleButton.onClick.AddListener(() => RecycleButtonCallback(randomObjectData)); // Add a new listener to handle button clicks
    }

    private void TakeButtonCallback(ObjectDataSO objectData)
    {
        m_playerObjects.AddObject(objectData); // Add the object to the player objects

        TryOpenChest(); // Try to open the next chest
    }

    private void RecycleButtonCallback(ObjectDataSO objectData)
    {
        CurrencyManager.Instance.AddCurrency(objectData.RecyclePrice); // Add currency to the player based on the recycle value of the object data

        TryOpenChest(); // Try to open the next chest
    }

    [Button()]
    private void ConfigureUpgradeContainers()
    {
        m_upgradeContainersParent.gameObject.SetActive(true); // Show the upgrade containers

        for (int i = 0; i < m_upgradeContainers.Length; i++)
        {
            int randomUpgradeIndex = UnityEngine.Random.Range(0, Enum.GetValues(typeof(Stat)).Length); // Randomly select an upgrade type
            Stat upgradeType = (Stat)randomUpgradeIndex; // Cast the random index to Stat enum
            string upgradeText = Enums.FormatStatName(upgradeType); // Create the upgrade text based on the selected type

            Sprite upgradeIconSprite = ResourcesManager.GetStatIcon(upgradeType); // Get the icon sprite for the selected upgrade type

            string buttonString;
            Action action = GetActionToPerform(upgradeType, out buttonString); // Get the action to perform on button click

            m_upgradeContainers[i].Configure(upgradeIconSprite, upgradeText, buttonString); // Set the text for each upgrade container

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
            case Stat.HealthRegeneration:
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
            PlayerStatManager.Instance.AddPlayerStat(stat, value); // Add the selected stat to the player stat manager
        };
    }

    private void ChestCollectedCallBack(Chest chest)
    {
        m_chestCollectedCount++; // Increment the chest collect count
    }

    public bool HasChestCollected()
    {
        if (m_chestCollectedCount > 0)
        {
            return true; // Return true if there are chests collected
        }
        else
        {
            return false; // Return false if there are no chests collected
        }
    }
}