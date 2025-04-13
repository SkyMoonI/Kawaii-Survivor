using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class WaveManager : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Player m_player;
    [SerializeField] private WaveUIManager m_waveUIManager; // Reference to the UI manager for updating wave and timer text

    [Header("Settings")]
    private float m_timer;
    private bool m_isTimerActive = false;
    private float m_currentWaveDuration;
    private int m_currentWaveIndex;


    [Header("Waves")]
    [SerializeField] private Wave[] m_waves;
    private List<float> m_localCounters = new List<float>();

    void Awake()
    {
        m_waveUIManager = GetComponent<WaveUIManager>(); // Get the WaveUIManager component attached to the same GameObject
    }
    void Start()
    {
        m_currentWaveIndex = 0;

        StartWave(m_currentWaveIndex); // Start the first wave
    }

    void Update()
    {
        if (!m_isTimerActive)
        {
            return;
        }


        if (m_timer < m_currentWaveDuration)
        {
            ManageCurrentWave();

            string timerText = ((int)(m_currentWaveDuration - m_timer)).ToString(); // Format the timer text to 2 decimal places
            m_waveUIManager.UpdateTimerText(timerText); // Update the timer text in the UI
        }
        else
        {
            StartWaveTransition(); // Start the transition to the next wave
        }
    }

    private void StartWave(int waveIndex)
    {
        string waveText = $"Wave {waveIndex + 1} / {m_waves.Length}"; // Format the wave text
        m_waveUIManager.UpdateWaveText(waveText); // Update the wave text in the UI

        m_localCounters.Clear(); // Clear the local counters for the new wave
        foreach (WaveSegment segment in m_waves[waveIndex].segments)
        {
            m_localCounters.Add(1f); // Initialize the local counters for each segment
        }

        m_timer = 0f;
        m_currentWaveDuration = m_waves[waveIndex].waveDuration;

        m_isTimerActive = true; // Start the timer
    }

    private void ManageCurrentWave()
    {
        Wave currentWave = m_waves[m_currentWaveIndex];

        for (int i = 0; i < currentWave.segments.Count; i++)
        {
            WaveSegment segment = currentWave.segments[i];
            float spawnStartTime = segment.timeStartEnd.x / 100f * m_currentWaveDuration; // convert to seconds
            float spawnEndTime = segment.timeStartEnd.y / 100f * m_currentWaveDuration; // convert to seconds

            if (m_timer < spawnStartTime || m_timer > spawnEndTime)
            {
                continue;
            }

            float timeSinceSegmentStart = m_timer - spawnStartTime; // how long since the start of the spawn time
            float spawnInterval = 1f / segment.spawnFrequency; // how often to spawn enemies

            if (timeSinceSegmentStart / spawnInterval >= m_localCounters[i])
            {
                // Spawn enemy
                Enemy enemy = Instantiate(segment.enemyPrefab, GetSpawnPosition(), Quaternion.identity, transform);
                enemy.gameObject.SetActive(true); // Activate the enemy game object

                m_localCounters[i]++; // Increment the local counter for this segment
            }
        }

        m_timer += Time.deltaTime; // Increment the timer
    }

    private void StartWaveTransition()
    {
        m_isTimerActive = false; // Stop the timer

        DefeatAllEnemies(); // Defeat all enemies in the current wave

        m_currentWaveIndex++; // Move to the next wave

        if (m_currentWaveIndex >= m_waves.Length)
        {
            m_waveUIManager.UpdateWaveText("All Waves Cleared!"); // Update the UI to indicate all waves are cleared
            m_waveUIManager.UpdateTimerText(""); // Reset the timer text to 0
        }
        else
        {
            StartWave(m_currentWaveIndex); // Start the new wave
        }
    }

    private void DefeatAllEnemies()
    {
        while (transform.childCount > 0)
        {
            Transform child = transform.GetChild(0); // Get the first child (enemy)
            child.SetParent(null); // Unparent the enemy from the wave manager
            Destroy(child.gameObject); // Destroy the enemy game object
        }
    }

    private Vector2 GetSpawnPosition()
    {
        Vector2 direction = Random.onUnitSphere; // Get a random direction on the unit sphere to spawn the enemy around the player
        Vector2 offset = direction.normalized * Random.Range(6f, 10f); // Random distance from the player
        Vector2 targetPosition = (Vector2)m_player.transform.position + offset; // Calculate the target position

        // Clamp the target position to ensure it stays within the bounds of the game area
        // Adjust these values based on your game area size
        targetPosition.x = Mathf.Clamp(targetPosition.x, -18f, 18f); // Clamp the x position
        targetPosition.y = Mathf.Clamp(targetPosition.y, -8f, 8f); // Clamp the y position

        return targetPosition; // Placeholder for spawn position logic
    }

}


[System.Serializable]
public struct Wave
{
    public string name;
    public int waveDuration; // how long the wave will last
    public List<WaveSegment> segments;
}

[System.Serializable]
public struct WaveSegment
{
    public Enemy enemyPrefab;
    [MinMaxSlider(0f, 100f)] public Vector2 timeStartEnd; // tweak the value to set the time when the enemy will spawn and when it will end
    public int spawnFrequency; // how many enemies will spawn in a second
}