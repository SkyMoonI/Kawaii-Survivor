using UnityEngine;
using Joystick;
using NUnit.Framework.Constraints;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour, IPlayerStatsDependency
{
    [Header("Elements")]
    [SerializeField] private Rigidbody2D m_rigidbody2D;
    [SerializeField] private MobileJoystick m_moveJoystick;

    [Header("Settings")]
    [SerializeField] private float m_baseMoveSpeed; // Base move speed
    [SerializeField] private float m_moveSpeed;


    void FixedUpdate()
    {
        m_rigidbody2D.linearVelocity = m_moveJoystick.GetMoveVector() * m_moveSpeed * Time.fixedDeltaTime;
    }


    public void UpdateStats(PlayerStatManager playerStatManager)
    {
        float moveSpeedPercentage = playerStatManager.GetStatValue(Stat.MoveSpeed) / 100; // Get the move speed percentage from the stat manager
        m_moveSpeed = m_baseMoveSpeed * (1 + moveSpeedPercentage); // Calculate the new move speed
    }
}
