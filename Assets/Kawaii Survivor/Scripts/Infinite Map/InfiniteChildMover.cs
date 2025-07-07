using UnityEngine;

public class InfiniteChildMover : MonoBehaviour
{
    private Transform m_playerTransform;

    [SerializeField] private float m_mapChunkSize = 50f;
    [SerializeField] private float m_distanceThreshold = .5f;

    void Start()
    {
        m_playerTransform = Player.Instance.transform;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateChildren();
    }

    private void UpdateChildren()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);

            Vector3 distance = m_playerTransform.position - child.position;

            float calculatedDistanceThreshold = m_mapChunkSize * m_distanceThreshold;

            int unitesToMove = 2;

            if (Mathf.Abs(distance.x) > calculatedDistanceThreshold)
            {
                child.position += Vector3.right * calculatedDistanceThreshold * unitesToMove * Mathf.Sign(distance.x);
            }

            if (Mathf.Abs(distance.y) > calculatedDistanceThreshold)
            {
                child.position += Vector3.up * calculatedDistanceThreshold * unitesToMove * Mathf.Sign(distance.y);
            }
        }
    }
}
