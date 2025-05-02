using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
public class HopOnTop : MonoBehaviour, IPointerDownHandler
{
    private RectTransform m_rectTransform;
    private Vector2 m_originalPosition;

    void Awake()
    {
        m_rectTransform = GetComponent<RectTransform>();
    }

    void Start()
    {
        m_originalPosition = m_rectTransform.anchoredPosition;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        float targetY = m_originalPosition.y + Screen.height / 50f;

        LeanTween.cancel(gameObject);
        LeanTween.moveY(m_rectTransform, targetY, 0.6f)
            .setEase(LeanTweenType.punch) // scale it like an elastic bounce
            .setIgnoreTimeScale(true); // even if timescale is set to 0 this will still work
    }
}
