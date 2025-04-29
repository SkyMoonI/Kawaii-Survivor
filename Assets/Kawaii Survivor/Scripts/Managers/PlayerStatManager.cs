using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class PlayerStatManager : MonoBehaviour
{
    public static PlayerStatManager Instance { get; private set; } // Singleton instance of PlayerStatManager

    [Header("Data")]
    [SerializeField] private CharacterDataSO m_characterData; // Reference to character data scriptable object

    [Header("Settings")]
    private Dictionary<Stat, StatData> m_playerStats = new Dictionary<Stat, StatData>();
    private Dictionary<Stat, StatData> m_addends = new Dictionary<Stat, StatData>();
    private Dictionary<Stat, float> m_objectAddends = new Dictionary<Stat, float>();


    void Awake()
    {
        if (Instance == null) // If no instance exists, set this as the instance
        {
            Instance = this;
        }
        else if (Instance != this) // If another instance exists, destroy this object
        {
            Destroy(gameObject);
        }

        m_playerStats = m_characterData.BaseStats; // Initialize player stats from character data

        foreach (KeyValuePair<Stat, StatData> stat in m_playerStats) // Initialize player stats from character data
        {
            m_addends.Add(stat.Key, new StatData(stat.Key, 0)); // Initialize addends to zero
            m_objectAddends.Add(stat.Key, 0);
        }
    }

    void Start()
    {
        UpdatePlayerStats(); // Notify listeners of the initial state
    }

    void OnEnable()
    {
        CharacterSelectionManager.onCharacterSelected += CharacterSelectedCallBack;
    }

    void OnDisable()
    {
        CharacterSelectionManager.onCharacterSelected -= CharacterSelectedCallBack;
    }

    void OnDestroy()
    {
        CharacterSelectionManager.onCharacterSelected -= CharacterSelectedCallBack;
    }

    public void AddPlayerStat(Stat stat, float value)
    {
        if (m_addends.ContainsKey(stat))
        {
            m_addends[stat] += new StatData(stat, value);
        }
        else
        {
            Debug.LogError($"Stat {stat} not found in addends dictionary. ");
        }

        UpdatePlayerStats();
    }

    public void AddObjectStat(Dictionary<Stat, float> objectStats)
    {
        foreach (KeyValuePair<Stat, float> stat in objectStats) // Initialize player stats from character data
        {
            m_objectAddends[stat.Key] += stat.Value; // Initialize addends to zero
        }

        UpdatePlayerStats();
    }

    public void RemoveObjectStat(Dictionary<Stat, float> objectStats)
    {
        foreach (KeyValuePair<Stat, float> stat in objectStats) // Initialize player stats from character data
        {
            m_objectAddends[stat.Key] -= stat.Value; // Initialize addends to zero
        }

        UpdatePlayerStats();
    }

    public float GetStatValue(Stat stat)
    {
        return m_playerStats[stat].value + m_addends[stat].value + m_objectAddends[stat]; // Calculate the total value of the stat;
    }

    private void UpdatePlayerStats()
    {
        IEnumerable<IPlayerStatsDependency> playerStatsDependencies =
        FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None)
        .OfType<IPlayerStatsDependency>(); // Find all listeners in the scene

        foreach (IPlayerStatsDependency dependency in playerStatsDependencies)
        {
            dependency.UpdateStats(Instance); // Notify each listener of the state change
        }
    }

    private void CharacterSelectedCallBack(CharacterDataSO characterData)
    {
        m_characterData = characterData;
        m_playerStats = m_characterData.BaseStats;

        UpdatePlayerStats();
    }
}
[System.Serializable]
public struct StatData
{
    public Stat stat;
    public float value;

    public StatData(Stat stat, float value)
    {
        this.stat = stat;
        this.value = value;
    }

    public static StatData operator +(StatData stat1, StatData stat2)
    {
        if (stat1.stat != stat2.stat)
        {
            Debug.LogError("Cannot add StatData with different stats.");
            return stat1;
        }
        return new StatData(stat1.stat, stat1.value + stat2.value);
    }

    public static StatData operator -(StatData stat1, StatData stat2)
    {
        if (stat1.stat != stat2.stat)
        {
            Debug.LogError("Cannot subtract StatData with different stats.");
            return stat1;
        }
        return new StatData(stat1.stat, stat1.value - stat2.value);
    }
}