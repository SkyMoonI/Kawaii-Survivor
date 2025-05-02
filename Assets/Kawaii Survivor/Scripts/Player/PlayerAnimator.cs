using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerAnimator : MonoBehaviour
{
    [Header("Elemenets")]
    private Animator m_animator;
    private Rigidbody2D m_rigidbody2D;

    void Awake()
    {
        m_animator = GetComponent<Animator>();
        m_rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (m_rigidbody2D.linearVelocity.magnitude < 0.001f)
        {
            m_animator.Play("Idle");
        }
        else
        {
            m_animator.Play("Move");
        }
    }


}
