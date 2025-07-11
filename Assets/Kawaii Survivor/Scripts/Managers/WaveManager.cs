using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class WaveManager : MonoBehaviour, IGameStateListener
{
    [Header("Elements")]
    [SerializeField] private Player m_player;
    private WaveUIManager m_waveUIManager; // Reference to the UI manager for updating wave and timer text

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
            m_localCounters.Add(0f); // Initialize the local counters for each segment
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

                // for spawn once (boss)
                if (segment.spawnOnce)
                {
                    m_localCounters[i] = Mathf.Infinity;
                }
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
            GameManager.Instance.SetGameState(GameState.STAGECOMPLETE); // Set the game state to stage complete
        }
        else
        {
            GameManager.Instance.WaveCompletedCallBack(); // Call the GameManager's wave completed callback
        }
    }

    private void StartNextWave()
    {
        StartWave(m_currentWaveIndex); // Start the next wave
    }

    private void DefeatAllEnemies()
    {
        foreach (Enemy enemy in transform.GetComponentsInChildren<Enemy>())
        {
            enemy.PassAwayAfterWave(); // Call the PassAway method on each enemy to defeat them
        }
    }

    private Vector2 GetSpawnPosition()
    {
        Vector2 direction = Random.onUnitSphere; // Get a random direction on the unit sphere to spawn the enemy around the player
        Vector2 offset = direction.normalized * Random.Range(6f, 10f); // Random distance from the player
        Vector2 targetPosition = (Vector2)m_player.transform.position + offset; // Calculate the target position

        if (!GameManager.Instance.IsUsingInfiniteMap)
        {
            // Clamp the target position to ensure it stays within the bounds of the game area
            // Adjust these values based on your game area size
            targetPosition.x = Mathf.Clamp(targetPosition.x, -Constants.arenaSize.x / 2f, Constants.arenaSize.x / 2f); // Clamp the x position
            targetPosition.y = Mathf.Clamp(targetPosition.y, -Constants.arenaSize.y / 2f, Constants.arenaSize.y / 2f); // Clamp the y position
        }

        return targetPosition; // Placeholder for spawn position logic
    }

    public void GameStateChangedCallBack(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.GAME:
                m_isTimerActive = true; // Start the timer when in game state
                StartNextWave();
                break;
            case GameState.GAMEOVER:
                m_isTimerActive = false; // Stop the timer when in game over state
                DefeatAllEnemies(); // Defeat all enemies when game is over
                break;
            default:
                break;
        }
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
    public bool spawnOnce;
}