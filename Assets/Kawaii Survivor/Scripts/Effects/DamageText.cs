using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    [Header("Elements")]
    private Animator m_animator;
    [SerializeField] private TMP_Text m_text; // text to display the damage amount

    public void Awake()
    {
        m_animator = GetComponent<Animator>();
    }
    public void Animate(string textToDisplay, bool isCriticalHit)
    {
        m_text.text = textToDisplay; // set the text to the damage amount

        m_text.color = isCriticalHit ? Color.red : Color.white; // set the text color based on whether it's a critical hit or not

        m_animator.Play("Animate");
    }


}
