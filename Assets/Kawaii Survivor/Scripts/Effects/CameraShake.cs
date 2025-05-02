using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float m_shakeMagnitude = 0.05f;
    [SerializeField] private float m_shakeDuration = 0.15f;

    void OnEnable()
    {
        RangeWeapon.onBulletShot += Shake;
    }

    void OnDisable()
    {
        RangeWeapon.onBulletShot -= Shake;
    }

    void OnDestroy()
    {
        RangeWeapon.onBulletShot -= Shake;
    }

    private void Shake()
    {
        Vector3 direction = Random.onUnitSphere.normalized;
        //direction.z = -10;

        //transform.localPosition = new Vector3(0, 0, -10);
        transform.localPosition = Vector3.zero;

        LeanTween.cancel(gameObject);
        LeanTween.moveLocal(gameObject, direction * m_shakeMagnitude, m_shakeDuration).setEase(LeanTweenType.easeShake);
    }
}
