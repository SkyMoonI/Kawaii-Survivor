using UnityEngine;

public class InfiniteMap : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private GameObject m_mapChunkPrefab;

    [Header("Settings")]
    [SerializeField] private float m_mapChunkSize = 50f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GenerateMap();
    }

    private void GenerateMap()
    {
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                GenerateMapChunk(x, y);
            }
        }
    }

    private void GenerateMapChunk(int x, int y)
    {
        Vector3 spawnPosition = new Vector3(x, y) * m_mapChunkSize;
        Instantiate(m_mapChunkPrefab, spawnPosition, Quaternion.identity, transform);
    }
}
