using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

[RequireComponent(typeof(RectTransform))]
public class ScaleAndRotate : MonoBehaviour, IPointerDownHandler
{
    private RectTransform m_rectTransform;

    void Awake()
    {
        m_rectTransform = GetComponent<RectTransform>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        LeanTween.cancel(gameObject);

        m_rectTransform.localScale = Vector2.one;

        LeanTween.scale(m_rectTransform, Vector2.one * 1.1f, 1f)
            .setEase(LeanTweenType.punch) // scale it like an go and come back
            .setIgnoreTimeScale(true); // even if timescale is set to 0 this will still work

        m_rectTransform.rotation = Quaternion.identity;
        int sign = (int)Mathf.Sign(Random.Range(-1f, 1f));
        LeanTween.rotateAround(m_rectTransform, Vector3.forward, 15f * sign, 1f)
            .setEase(LeanTweenType.punch) // rotate it like an go and come back
            .setIgnoreTimeScale(true); // even if timescale is set to 0 this will still work
    }
}
