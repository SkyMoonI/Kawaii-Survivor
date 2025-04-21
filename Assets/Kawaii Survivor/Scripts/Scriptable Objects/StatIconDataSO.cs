using NUnit.Framework;
using UnityEngine;

[CreateAssetMenu(fileName = "New Stat Icons", menuName = "Scriptable Objects/New Stat Icons", order = 1)]
public class StatIconDataSO : ScriptableObject
{
    [field: SerializeField] public StatIcon[] StatIcons { get; private set; }

}

[System.Serializable]
public struct StatIcon
{
    public Stat stat;
    public Sprite icon;
}