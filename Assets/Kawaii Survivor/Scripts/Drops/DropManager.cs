using UnityEngine;
using UnityEngine.Pool;

public class DropManager : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Candy m_candyPrefab; // prefab to spawn when enemy dies
    [SerializeField] private Cash m_cashPrefab; // prefab to spawn when enemy dies
    [SerializeField] private Chest m_chestPrefab; // prefab to spawn when enemy dies

    [Header("Settings")]
    [SerializeField][Range(0f, 100f)] private float m_cashSpawnChance; // chance to spawn candy when an enemy dies
    [SerializeField][Range(0f, 100f)] private float m_chestSpawnChance; // chance to spawn a chest when an enemy dies


    void OnEnable()
    {
        Enemy.onEnemyDeath += EnemyPassAwayCallBack; // Subscribe to the enemy death event
        Enemy.onBossDeath += BossPassAwayCallBack;
        Candy.onCollected += ReleaseCandy; // Subscribe to the candy collected event
        Cash.onCollected += ReleaseCash; // Subscribe to the cash collected event
        Chest.onCollected += ReleaseChest; // Subscribe to the chest collected event
    }

    void OnDisable()
    {
        Enemy.onEnemyDeath -= EnemyPassAwayCallBack; // Unsubscribe from the enemy death event
        Enemy.onBossDeath -= BossPassAwayCallBack;
        Candy.onCollected -= ReleaseCandy; // Unsubscribe from the candy collected event
        Cash.onCollected -= ReleaseCash; // Unsubscribe from the cash collected event
        Chest.onCollected -= ReleaseChest; // Unsubscribe from the chest collected event
    }

    void OnDestroy()
    {
        Enemy.onEnemyDeath -= EnemyPassAwayCallBack; // Unsubscribe from the enemy death event
        Enemy.onBossDeath -= BossPassAwayCallBack;
        Candy.onCollected -= ReleaseCandy; // Unsubscribe from the candy collected event
        Cash.onCollected -= ReleaseCash; // Unsubscribe from the cash collected event
        Chest.onCollected -= ReleaseChest; // Unsubscribe from the chest collected event
    }

    [Header("Pooling")]
    private ObjectPool<Candy> m_candyPool;
    private ObjectPool<Cash> m_cashPool;
    private ObjectPool<Chest> m_chestPool; // Pool for chests

    void Awake()
    {
        m_candyPool = new ObjectPool<Candy>(CreateCandy, OnGetCandy, OnReleaseCandy, OnDestroyCandy);
        m_cashPool = new ObjectPool<Cash>(CreateCash, OnGetCash, OnReleaseCash, OnDestroyCash);
        m_chestPool = new ObjectPool<Chest>(CreateChest, OnGetChest, OnReleaseChest, OnDestroyChest); // Initialize the chest pool
    }

    private Candy CreateCandy() => Instantiate(m_candyPrefab, transform);
    private void OnGetCandy(Candy candy) => candy.gameObject.SetActive(true);
    private void OnReleaseCandy(Candy candy) => candy.gameObject.SetActive(false);
    private void OnDestroyCandy(Candy candy) => Destroy(candy.gameObject);

    private Cash CreateCash() => Instantiate(m_cashPrefab, transform);
    private void OnGetCash(Cash cash) => cash.gameObject.SetActive(true);
    private void OnReleaseCash(Cash cash) => cash.gameObject.SetActive(false);
    private void OnDestroyCash(Cash cash) => Destroy(cash.gameObject);

    private Chest CreateChest() => Instantiate(m_chestPrefab, transform); // Create a new chest instance
    private void OnGetChest(Chest chest) => chest.gameObject.SetActive(true); // Activate the chest when it's retrieved from the pool
    private void OnReleaseChest(Chest chest) => chest.gameObject.SetActive(false); // Deactivate the chest when it's released from the pool
    private void OnDestroyChest(Chest chest) => Destroy(chest.gameObject); // Destroy the chest when it's destroyed

    public void ReleaseCandy(Candy candy) => m_candyPool.Release(candy);
    public void ReleaseCash(Cash cash) => m_cashPool.Release(cash);
    public void ReleaseChest(Chest chest) => m_chestPool.Release(chest); // Release the chest back to the pool


    private void EnemyPassAwayCallBack(Vector2 enemyPosition)
    {
        bool shouldSpawnCash = Random.Range(0f, 100f) <= m_cashSpawnChance; // Randomly decide whether to spawn cash or candy based on the spawn chance

        DroppableCurrency collectable = shouldSpawnCash ? m_cashPool.Get() : m_candyPool.Get(); // Randomly choose between candy and cash prefab
        collectable.transform.position = enemyPosition; // Set the position of the collectable to the enemy position

        TrySpawnChest(enemyPosition); // Try to spawn a chest at the enemy position
    }

    private void BossPassAwayCallBack(Vector2 bossPosition)
    {
        DropChest(bossPosition); // spawn a chest at the boss position
    }

    private void TrySpawnChest(Vector2 enemyPosition)
    {
        bool shouldSpawnChest = Random.Range(0f, 100f) <= m_chestSpawnChance; // Randomly decide whether to spawn cash or candy based on the spawn chance

        if (!shouldSpawnChest) // If the chest should not be spawned, return
            return; // Exit the method

        DropChest(enemyPosition);
    }

    private void DropChest(Vector2 enemyPosition)
    {
        Chest chest = m_chestPool.Get(); // Get a chest instance from the pool
        chest.transform.position = enemyPosition; // Set the position of the chest to the enemy position
    }
}
