using System;
using UnityEngine;
using UnityEngine.PlayerLoop;

[RequireComponent(typeof(PlayerHealth))]
[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(PlayerLevel))]
public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    [Header("Elements")]
    private PlayerHealth m_playerHealth;
    private CircleCollider2D m_playerCollider; // reference to the player collider
    private PlayerLevel m_playerLevel; // reference to the player level
    [SerializeField] private SpriteRenderer m_playerSpriteRenderer;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
        }

        m_playerHealth = GetComponent<PlayerHealth>();
        m_playerCollider = GetComponent<CircleCollider2D>(); // get the CircleCollider2D component
        m_playerLevel = GetComponent<PlayerLevel>();
    }

    void OnEnable()
    {
        CharacterSelectionManager.onCharacterSelected += CharacterSelectedCallBack;
    }



    void OnDisable()
    {
        CharacterSelectionManager.onCharacterSelected -= CharacterSelectedCallBack;
    }

    void OnDestroy()
    {
        CharacterSelectionManager.onCharacterSelected -= CharacterSelectedCallBack;
    }

    private void CharacterSelectedCallBack(CharacterDataSO characterData)
    {
        m_playerSpriteRenderer.sprite = characterData.Sprite;
    }

    public void TakeDamage(float damage)
    {
        m_playerHealth.TakeDamage(damage);
    }

    public Vector2 GetCenterPosition()
    {
        return m_playerCollider.bounds.center; // get the center position of the player collider
    }

    public bool HasLevelUp()
    {
        return m_playerLevel.HasLevelUp();
    }
}
