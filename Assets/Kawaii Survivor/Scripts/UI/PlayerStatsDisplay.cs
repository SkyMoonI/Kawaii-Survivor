using System;
using UnityEngine;

public class PlayerStatsDisplay : MonoBehaviour, IPlayerStatsDependency
{
    [Header("Elements")]
    [SerializeField] private Transform m_playerStatContainersParent; // Array of stat containers for displaying stats
    public void UpdateStats(PlayerStatManager playerStatManager)
    {
        int index = 0; // Initialize index for stat containers

        foreach (Stat stat in Enum.GetValues(typeof(Stat))) // Loop through all stats
        {
            StatContainer statContainer = m_playerStatContainersParent.GetChild(index).GetComponent<StatContainer>(); // Get the stat container.
            statContainer.gameObject.SetActive(true); // Activate the stat container if it is not already active
            index++; // Increment index for next stat container

            Sprite statIconSprite = ResourcesManager.GetStatIcon(stat); // Get the icon sprite for the stat

            float statValue = playerStatManager.GetStatValue(stat); // Get the value of the stat

            statContainer.Configure(statIconSprite, Enums.FormatStatName(stat), statValue, true); // Configure the stat container with the icon and value
        }
    }
}
