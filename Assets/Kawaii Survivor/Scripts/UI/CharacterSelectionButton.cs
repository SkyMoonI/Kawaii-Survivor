using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectionButton : MonoBehaviour
{
    [SerializeField] private Image m_characterImage;
    [SerializeField] private GameObject m_lockObject;

    public Button Button { get { return GetComponent<Button>(); } private set { } }

    public void Configure(Sprite characterIcon, bool unlocked)
    {
        m_characterImage.sprite = characterIcon;

        if (!unlocked)
        {
            Lock();
        }
        else
        {
            UnLock();
        }
    }

    public void Lock()
    {
        m_lockObject.SetActive(true);
        m_characterImage.color = Color.grey;
    }

    public void UnLock()
    {
        m_lockObject.SetActive(false);
        m_characterImage.color = Color.white;
    }
}
