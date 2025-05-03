using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RangeEnemyAttack))]
public class Zoomby : Enemy
{
    [Header("Elements")]
    [SerializeField] private Slider m_healthSlider;
    [SerializeField] private TMP_Text m_text;
    [SerializeField] private Animator m_animator;

    enum State { None, Idle, Moving, Attacking }

    [Header(" State Machine ")]
    private State m_state;
    private float m_timer;

    [Header(" Idle State ")]
    [SerializeField] private float m_maxIdleDuration;
    private float m_idleDuration;

    [Header(" Moving State ")]
    [SerializeField] private float m_moveSpeed;
    private Vector2 m_targetPosition;

    [Header(" Attack State ")]
    private int m_attackCounter;
    private RangeEnemyAttack m_attack;

    protected override void Awake()
    {
        base.Awake();

        m_state = State.None;

        m_attack = GetComponent<RangeEnemyAttack>();

        m_healthSlider.gameObject.SetActive(false);
        m_healthSlider.value = 1f; // Set the initial value to 1 (full health)

    }

    protected override void Start()
    {
        base.Start();
    }

    void Update()
    {
        ManageStates();
    }

    void OnEnable()
    {
        onSpawnSequenceCompleted += SpawnSequenceCompletedCallback;
        onDamageTaken += DamageTakenCallback;
    }

    void OnDisable()
    {
        onSpawnSequenceCompleted -= SpawnSequenceCompletedCallback;
        onDamageTaken -= DamageTakenCallback;
    }

    void OnDestroy()
    {
        onSpawnSequenceCompleted -= SpawnSequenceCompletedCallback;
        onDamageTaken -= DamageTakenCallback;
    }

    private void ManageStates()
    {
        switch (m_state)
        {
            case State.Idle:
                ManageIdleState();
                break;

            case State.Moving:
                ManageMovingState();
                break;

            case State.Attacking:
                ManageAttackingState();
                break;

            default:
                break;
        }
    }

    private void SetIdleState()
    {
        Debug.Log("Started Idle");

        m_state = State.Idle;
        m_idleDuration = Random.Range(1f, m_maxIdleDuration);

        Debug.Log("Timer value : " + m_timer);

        m_animator.Play("Idle");
    }

    private void ManageIdleState()
    {
        m_timer += Time.deltaTime;

        if (m_timer >= m_idleDuration)
        {
            m_timer = 0;
            StartMovingState();
        }
    }

    private void StartMovingState()
    {
        Debug.Log("Started Moving");

        m_state = State.Moving;
        m_targetPosition = GetRandomPosition();

        Debug.Log("Target Position : " + m_targetPosition);

        m_animator.Play("Move");
    }

    private void ManageMovingState()
    {
        transform.position = Vector2.MoveTowards(transform.position, m_targetPosition, m_moveSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, m_targetPosition) < .01f)
            StartAttackingState();
    }

    private void StartAttackingState()
    {
        Debug.Log("Started Attacking");
        m_state = State.Attacking;
        m_attackCounter = 0;

        m_animator.Play("Attack");
    }

    private void ManageAttackingState()
    {

    }

    private void Attack()
    {
        Vector2 direction = Quaternion.Euler(0, 0, -45 * m_attackCounter) * Vector2.up;
        m_attack.InstantShoot(direction);
        m_attackCounter++;
    }

    public void UpdateHealth(float currentHealth, float maxHealth)
    {
        if (maxHealth > 0)
        {
            m_healthSlider.value = (float)currentHealth / maxHealth; // Update the slider value based on current and max health
            UpdateText(currentHealth, maxHealth);
        }
    }

    private void UpdateText(float currentHealth, float maxHealth)
    {
        if (m_text != null)
        {
            m_text.text = $"{(int)currentHealth} / {maxHealth}"; // Update the text to show current and max health
        }
    }

    private void SpawnSequenceCompletedCallback()
    {
        m_healthSlider.gameObject.SetActive(true);
        UpdateHealth(m_currentHealth, m_maxHealth);

        SetIdleState();
    }

    public void DamageTakenCallback(float realDamage, Vector2 position, bool isCriticalHit)
    {
        UpdateHealth(m_currentHealth, m_maxHealth); // Update the health bar
    }

    protected override void PassAway()
    {
        onBossDeath?.Invoke(transform.position);

        PassAwayAfterWave();
    }

    private Vector2 GetRandomPosition()
    {
        Vector2 targetPosition = Vector2.zero;

        targetPosition.x = Random.Range(-Constants.arenaSize.x / 3f, Constants.arenaSize.x / 3f); // Clamp the x position
        targetPosition.y = Random.Range(-Constants.arenaSize.y / 3f, Constants.arenaSize.y / 3f); // Clamp the y position

        return targetPosition;
    }

    protected override void TryAttack()
    {
    }
}
