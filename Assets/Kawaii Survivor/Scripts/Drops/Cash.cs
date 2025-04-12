using System;
using System.Collections;
using UnityEngine;

public class Cash : DroppableCurrency
{
    [Header("Actions")]
    public static Action<Cash> onCollected; // action to call when the candy is collected

    protected override void Collected()
    {
        onCollected?.Invoke(this); // invoke the action to notify that the candy is collected
    }
}
