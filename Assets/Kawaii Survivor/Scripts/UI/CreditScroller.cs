using UnityEngine;

public class CreditScroller : MonoBehaviour
{
    [SerializeField] private RectTransform m_rectTransform;
    [SerializeField] private float m_speed;

    void OnEnable()
    {
        m_rectTransform.anchoredPosition = Vector2.zero;
    }

    void Update()
    {
        if (m_rectTransform.anchoredPosition.y < m_rectTransform.sizeDelta.y)
        {
            m_rectTransform.anchoredPosition += Vector2.up * m_speed * Time.deltaTime; // Move the RectTransform down at a constant speed
        }
    }
}
