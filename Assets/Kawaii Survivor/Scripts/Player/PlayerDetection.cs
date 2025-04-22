using UnityEngine;
using UnityEngine.Rendering;

public class PlayerDetection : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private CircleCollider2D m_playerCollectableCollider; // detection area of the player

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out ICollectable collectable))
        {
            if (!other.IsTouching(m_playerCollectableCollider))
            {
                return;
            }

            collectable.Collect(); // collect the candy
        }
    }
}
