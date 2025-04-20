using UnityEngine;

[CreateAssetMenu(fileName = "New Palette Data", menuName = "Scriptable Objects/New Palette Data", order = 1)]
public class PaletteSO : ScriptableObject
{
    [field: SerializeField] public Color[] LevelColors { get; private set; }
    [field: SerializeField] public Color[] LevelOutlineColors { get; private set; }
}
