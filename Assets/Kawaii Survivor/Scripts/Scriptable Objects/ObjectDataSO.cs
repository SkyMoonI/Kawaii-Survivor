using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "New Object Data", menuName = "Scriptable Objects/New Object Data", order = 1)]
public class ObjectDataSO : ScriptableObject
{
    [field: SerializeField] public string Name { get; private set; } // Character name
    [field: SerializeField] public Sprite IconSprite { get; private set; } // Character sprite
    [field: SerializeField] public int PurchasePrice { get; private set; } // Character purchase price
    [field: SerializeField] public int RecyclePrice { get; private set; } // Character purchase price
    [field: Range(0, 3)][field: SerializeField] public int Rarity { get; private set; } // Character sell price


    [field: SerializeField] public StatData[] StatDatas { get; private set; }

    public Dictionary<Stat, float> BaseStats
    {
        get
        {
            Dictionary<Stat, float> stats = new Dictionary<Stat, float>();
            foreach (StatData statData in StatDatas)
            {
                stats.Add(statData.stat, statData.value);
            }
            return stats;
        }

        private set { }
    }
}
