using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    [Header("Elements")]
    private Animator m_animator;
    [SerializeField] private TMP_Text m_damageText; // text to display the damage amount

    public void Awake()
    {
        m_animator = GetComponent<Animator>();
    }
    public void Animate(float damage, bool isCriticalHit)
    {
        m_damageText.text = damage.ToString(); // set the text to the damage amount

        m_damageText.color = isCriticalHit ? Color.red : Color.white; // set the text color based on whether it's a critical hit or not

        m_animator.Play("Animate");
    }
}
