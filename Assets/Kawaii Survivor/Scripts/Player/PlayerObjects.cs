using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerStatManager))]
public class PlayerObjects : MonoBehaviour
{
    [field: SerializeField] public List<ObjectDataSO> Objects { get; private set; } // List of objects
    private PlayerStatManager m_playerStatManager; // Reference to PlayerStatManager

    private void Awake()
    {
        m_playerStatManager = GetComponent<PlayerStatManager>(); // Get the PlayerStatManager component
    }

    void Start()
    {
        foreach (ObjectDataSO objectData in Objects) // Initialize player stats from character data
        {
            m_playerStatManager.AddObjectStat(objectData.BaseStats); // Add object stats to player stats
        }
    }

    public void AddObject(ObjectDataSO objectData) // Add an object to the player
    {
        //if (Objects.Contains(objectData)) return; // Check if the object is already in the list

        Objects.Add(objectData); // Add the object to the list
        m_playerStatManager.AddObjectStat(objectData.BaseStats); // Add object stats to player stats
    }

    public void RecycleObject(ObjectDataSO objectData) // Recycle an object from the player
    {
        Objects.Remove(objectData); // Remove the object from the list

        CurrencyManager.Instance.AddCurrency(objectData.RecyclePrice); // Add currency to the player based on the recycle value of the object data

        m_playerStatManager.RemoveObjectStat(objectData.BaseStats); // Remove object stats from player stats
    }
}
