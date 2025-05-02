using UnityEngine;

public class HapticFeedback : MonoBehaviour
{
    void OnEnable()
    {
        RangeWeapon.onBulletShot += Vibrate;
    }

    void OnDisable()
    {
        RangeWeapon.onBulletShot -= Vibrate;
    }

    void OnDestroy()
    {
        RangeWeapon.onBulletShot -= Vibrate;
    }

    private void Vibrate()
    {
        CandyCoded.HapticFeedback.HapticFeedback.HeavyFeedback();
    }
}
