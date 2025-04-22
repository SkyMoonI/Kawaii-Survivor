using UnityEngine;

public class WeaponSelectionManager : MonoBehaviour, IGameStateListener
{
    [Header("Elements")]
    [SerializeField] private Transform m_containerParent;
    [SerializeField] private WeaponSelectionContainer m_weaponSelectionContainerPrefab;
    [SerializeField] private PlayerWeapons m_playerWeapons; // Reference to the PlayerWeapons script

    [Header("Settings")]
    private WeaponDataSO m_selectedWeapon; // Reference to the selected weapon data
    private int initialWeaponLevel;

    [Header("Scriptable Objects")]
    [SerializeField] private WeaponDataSO[] m_starterWeapons;

    public void GameStateChangedCallBack(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.GAME:

                if (m_selectedWeapon == null)
                {
                    return;
                }

                m_playerWeapons.AddWeapon(m_selectedWeapon, initialWeaponLevel);
                initialWeaponLevel = 0;
                m_selectedWeapon = null; // Reset the selected weapon after adding it to the player weapons

                break;
            case GameState.WEAPONSELECTION:
                Configure();
                break;
        }
    }

    [NaughtyAttributes.Button("Configure")]
    private void Configure()
    {
        // Destroy all children of the container parent
        while (m_containerParent.childCount > 0)
        {
            Transform child = m_containerParent.GetChild(0); // Get the first child (enemy)
            child.SetParent(null); // Unparent the enemy from the wave manager
            Destroy(child.gameObject); // Destroy the enemy game object
        }

        // Create a new weapon selection container to select a weapon
        for (int i = 0; i < 3; i++)
        {
            GenerateWeaponSelectionContainer();
        }
    }

    private void GenerateWeaponSelectionContainer()
    {
        WeaponSelectionContainer container = Instantiate(m_weaponSelectionContainerPrefab, m_containerParent);

        WeaponDataSO weaponData = m_starterWeapons[Random.Range(0, m_starterWeapons.Length)];

        int level = Random.Range(0, 4); // Randomly select a level between 0 and 3

        // Configure the container with the weapon data
        container.Configure(weaponData, level); // Replace with actual weapon data

        container.Button.onClick.RemoveAllListeners(); // Remove all listeners from the button
        container.Button.onClick.AddListener(() => ContainerSelectedCallback(container, weaponData, level)); // Add a new listener to handle button clicks

    }

    private void ContainerSelectedCallback(WeaponSelectionContainer containerInstance, WeaponDataSO weaponData, int level)
    {
        m_selectedWeapon = weaponData; // Set the selected weapon data
        initialWeaponLevel = level;

        foreach (WeaponSelectionContainer container in m_containerParent.GetComponentsInChildren<WeaponSelectionContainer>())
        {
            if (container == containerInstance)
            {
                container.Select(); // Select the clicked container
            }
            else
            {
                container.Deselect(); // Deselect other containers
            }
        }
    }
}
