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
    public void Animate(float damage)
    {
        m_damageText.text = damage.ToString(); // set the text to the damage amount
        m_animator.Play("Animate");
    }
}
