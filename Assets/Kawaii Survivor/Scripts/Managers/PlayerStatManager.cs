using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerStatManager : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private CharacterDataSO m_characterData; // Reference to character data scriptable object

    [Header("Settings")]
    private Dictionary<Stat, StatData> m_playerStats = new Dictionary<Stat, StatData>();
    private Dictionary<Stat, StatData> m_addends = new Dictionary<Stat, StatData>();

    void Awake()
    {
        m_playerStats = m_characterData.BaseStats; // Initialize player stats from character data

        foreach (KeyValuePair<Stat, StatData> stat in m_playerStats) // Initialize player stats from character data
        {
            m_addends.Add(stat.Key, new StatData(stat.Key, 0)); // Initialize addends to zero
        }
    }

    void Start()
    {

        UpdatePlayerStats(); // Notify listeners of the initial state
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

    public float GetStatValue(Stat stat)
    {
        return m_playerStats[stat].m_value + m_addends[stat].m_value; // Calculate the total value of the stat;
    }

    private void UpdatePlayerStats()
    {
        IEnumerable<IPlayerStatsDependency> playerStatsDependencies =
        FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
        .OfType<IPlayerStatsDependency>(); // Find all listeners in the scene

        foreach (IPlayerStatsDependency dependency in playerStatsDependencies)
        {
            dependency.UpdateStats(this); // Notify each listener of the state change
        }
    }
}





public struct StatData
{
    public Stat m_stat;
    public float m_value;

    public StatData(Stat stat, float value)
    {
        m_stat = stat;
        m_value = value;
    }

    public static StatData operator +(StatData stat1, StatData stat2)
    {
        if (stat1.m_stat != stat2.m_stat)
        {
            Debug.LogError("Cannot add StatData with different stats.");
            return stat1;
        }
        return new StatData(stat1.m_stat, stat1.m_value + stat2.m_value);
    }

    public static StatData operator -(StatData stat1, StatData stat2)
    {
        if (stat1.m_stat != stat2.m_stat)
        {
            Debug.LogError("Cannot subtract StatData with different stats.");
            return stat1;
        }
        return new StatData(stat1.m_stat, stat1.m_value - stat2.m_value);
    }
}