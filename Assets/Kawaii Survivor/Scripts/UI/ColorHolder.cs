using UnityEngine;

public class ColorHolder : MonoBehaviour
{
    public static ColorHolder Instance { get; private set; } // Singleton instance of ColorHolder

    [Header("Scriptable Objects")]
    [SerializeField] private PaletteSO m_paletteSO; // Reference to the PaletteSO scriptable object

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this; // Set the singleton instance to this object
        }
        else
        {
            Destroy(gameObject); // Destroy this object if another instance already exists
        }
    }

    public Color GetColor(int level)
    {
        level = Mathf.Clamp(level, 0, m_paletteSO.LevelColors.Length); // Clamp the level to a valid range
        return Instance.m_paletteSO.LevelColors[level]; // Get the color for the specified level from the PaletteSO
    }

    public Color GetOutlineColor(int level)
    {
        level = Mathf.Clamp(level, 0, m_paletteSO.LevelOutlineColors.Length); // Clamp the level to a valid range
        return Instance.m_paletteSO.LevelOutlineColors[level]; // Get the color for the specified level from the PaletteSO
    }
}
