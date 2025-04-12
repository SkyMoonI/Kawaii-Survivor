using UnityEngine;

public class DropManager : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Candy m_candyPrefab; // prefab to spawn when enemy dies

    void OnEnable()
    {
        Enemy.onEnemyDeath += EnemyPassAwayCallBack; // Subscribe to the enemy death event
    }

    void OnDisable()
    {
        Enemy.onEnemyDeath -= EnemyPassAwayCallBack; // Unsubscribe from the enemy death event
    }

    void OnDestroy()
    {
        Enemy.onEnemyDeath -= EnemyPassAwayCallBack; // Unsubscribe from the enemy death event
    }

    private void EnemyPassAwayCallBack(Vector2 position)
    {
        // Spawn a candy at the enemy's position and set its parent to the DropManager
        Candy candy = Instantiate(m_candyPrefab, position, Quaternion.identity, transform);
    }
}
