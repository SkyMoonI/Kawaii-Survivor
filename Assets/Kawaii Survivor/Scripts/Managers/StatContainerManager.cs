using System.Collections.Generic;
using UnityEngine;

public class StatContainerManager : MonoBehaviour
{
    public static StatContainerManager Instance { get; private set; } // Singleton instance of the StatContainerManager

    [Header("Elements")]
    [SerializeField] private StatContainer m_statContainerPrefab; // Prefab for the stat container

    void Awake()
    {
        if (Instance == null) // Check if the instance is null
        {
            Instance = this; // Assign this instance to the singleton instance
        }
        else
        {
            Destroy(gameObject); // Destroy this instance if another one already exists
        }
    }

    private void GenerateContainers(Dictionary<Stat, float> baseStats, Transform parent)
    {
        List<StatContainer> statContainers = new List<StatContainer>(); // List to hold the stat containers

        foreach (KeyValuePair<Stat, float> stat in baseStats) // Iterate through the base stats of the weapon data
        {
            StatContainer statContainer = Instantiate(m_statContainerPrefab, parent); // Instantiate the stat container prefab

            statContainers.Add(statContainer); // Add the stat container to the list

            Sprite icon = ResourcesManager.GetStatIcon(stat.Key); // Get the icon for the stat from the ResourcesManager
            string name = Enums.FormatStatName(stat.Key); // Format the stat name using the Enums class
            float value = stat.Value; // Get the value of the stat as a string

            statContainer.Configure(icon, name, value); // Configure the stat container
        }

        LeanTween.delayedCall(Time.deltaTime * 2, () => ResizeFontSize(statContainers)); // Delay the resizing of font sizes for two frames to allow for animations to complete
    }

    private void ResizeFontSize(List<StatContainer> statContainers)
    {
        float minFontSize = float.MaxValue; // Initialize the minimum font size to the maximum value

        for (int i = 0; i < statContainers.Count; i++)
        {
            if (statContainers[i].GetFontSize() < minFontSize)
            {
                minFontSize = statContainers[i].GetFontSize(); // Find the minimum font size among the stat containers
            }
        }

        for (int i = 0; i < statContainers.Count; i++)
        {
            statContainers[i].SetFontSize(minFontSize); // Set the font size of each stat container to the minimum font size
        }

    }

    public static void GenerateStatContainers(Dictionary<Stat, float> calculatedBaseStats, Transform parent)
    {
        if (Instance == null) // Check if the instance is null
        {
            Debug.LogError("StatContainerManager instance is null. Make sure it is initialized before calling this method."); // Log an error if the instance is null
            return; // Exit the method if the instance is null
        }
        parent.Clear(); // Destroy all the children of the stat containers parent container before generating new ones

        Instance.GenerateContainers(calculatedBaseStats, parent); // Call the GenerateStatContainers method on the singleton instance
    }
}
