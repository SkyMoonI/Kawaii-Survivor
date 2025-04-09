using UnityEngine;

public class DamageTextManager : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private DamageText m_damageTextPrefab; // prefab to spawn when enemy dies

    void OnEnable()
    {
        Enemy.onDamageTaken += InstantiateDamageText;
    }
    void OnDisable()
    {
        Enemy.onDamageTaken -= InstantiateDamageText;
    }
    void OnDestroy()
    {
        Enemy.onDamageTaken -= InstantiateDamageText;
    }

    private void InstantiateDamageText(float damage, Vector2 enemyPosition)
    {
        Vector2 spawnPosition = enemyPosition + Vector2.up * 1.5f; // spawn position above the enemy
        DamageText damageTextInstance = Instantiate(m_damageTextPrefab, spawnPosition, Quaternion.identity, transform); // Instantiate the prefab at the enemy's position
        damageTextInstance.Animate(damage); // Call the Animate method to play the animation
    }

}
