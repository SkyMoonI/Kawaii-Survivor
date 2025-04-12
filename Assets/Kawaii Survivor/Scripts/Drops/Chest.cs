using System;
using UnityEngine;

public class Chest : MonoBehaviour, ICollectable
{
    [Header("Actions")]
    public static Action<Chest> onCollected; // action to call when the candy is collected

    public void Collect(Player player)
    {
        onCollected?.Invoke(this); // invoke the action to notify that the candy is collected
    }
}
