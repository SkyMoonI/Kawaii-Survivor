using UnityEngine;


[RequireComponent(typeof(SpriteRenderer))]
public class SpriteSorter : MonoBehaviour
{
    private SpriteRenderer m_spriteRenderer;


    void Awake()
    {
        m_spriteRenderer = GetComponent<SpriteRenderer>();
    }


    void Update()
    {
        m_spriteRenderer.sortingOrder = -(int)(transform.position.y * 10);
    }
}
