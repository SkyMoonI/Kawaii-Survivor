using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopManagerUI : MonoBehaviour
{
    [Header("Stats Panel Elements")]
    [SerializeField] private RectTransform m_playerStatsPanel;
    [SerializeField] private RectTransform m_playerStatsClosePanel;
    [SerializeField] private Button m_playerStatsOpenButton;
    [SerializeField] private EventTrigger m_playerStatsCloseButton;
    private Vector2 m_playerStatsPanelOpenPosition;
    private Vector2 m_playerStatsPanelClosedPosition;

    [Header("Inventory Panel Elements")]
    [SerializeField] private RectTransform m_playerInventoryPanel;
    [SerializeField] private RectTransform m_playerInventoryClosePanel;
    [SerializeField] private Button m_playerInventoryOpenButton;
    [SerializeField] private EventTrigger m_playerInventoryCloseButton;
    private Vector2 m_playerInventoryPanelOpenPosition;
    private Vector2 m_playerInventoryPanelClosedPosition;

    [Header("Item Info Slide Panel Elements")]
    [SerializeField] private RectTransform m_playerItemInfoPanel;
    [SerializeField] private RectTransform m_playerItemInfoClosePanel;
    [SerializeField] private Button m_playerItemInfoOpenButton;
    [SerializeField] private EventTrigger m_playerItemInfoCloseButton;
    private Vector2 m_playerItemInfoPanelOpenPosition;
    private Vector2 m_playerItemInfoPanelClosedPosition;


    void Start()
    {
        ConfigurePlayerStatsPanel();
        ConfigurePlayerInventoryPanel();
        ConfigurePlayerItemInfoPanel();
    }

    void OnEnable()
    {
        m_playerStatsOpenButton.onClick.AddListener(() => ShowPlayerPanel(m_playerStatsPanel, m_playerStatsClosePanel, m_playerStatsPanelOpenPosition));

        EventTrigger.Entry statsEntry = new EventTrigger.Entry();
        statsEntry.eventID = EventTriggerType.PointerDown;
        statsEntry.callback.AddListener((data) => { HidePlayerPanel(m_playerStatsPanel, m_playerStatsClosePanel, m_playerStatsPanelClosedPosition); });
        m_playerStatsCloseButton.triggers.Add(statsEntry);
        m_playerStatsCloseButton.enabled = true; // Ensure the Event Trigger is enabled

        m_playerInventoryOpenButton.onClick.AddListener(() => ShowPlayerPanel(m_playerInventoryPanel, m_playerInventoryClosePanel, m_playerInventoryPanelOpenPosition));

        EventTrigger.Entry inventoryEntry = new EventTrigger.Entry();
        inventoryEntry.eventID = EventTriggerType.PointerDown;
        inventoryEntry.callback.AddListener((data) => { HidePlayerPanel(m_playerInventoryPanel, m_playerInventoryClosePanel, m_playerInventoryPanelClosedPosition); });
        m_playerInventoryCloseButton.triggers.Add(inventoryEntry);
        m_playerInventoryCloseButton.enabled = true; // Ensure the Event Trigger is enabled

    }

    void OnDisable()
    {
        m_playerStatsOpenButton.onClick.RemoveAllListeners();
        m_playerInventoryOpenButton.onClick.RemoveAllListeners();

        m_playerStatsCloseButton.triggers.Clear();
        m_playerInventoryCloseButton.triggers.Clear();
    }

    void OnDestroy()
    {
        m_playerStatsOpenButton.onClick.RemoveAllListeners();
        m_playerInventoryOpenButton.onClick.RemoveAllListeners();

        m_playerStatsCloseButton.triggers.Clear();
        m_playerInventoryCloseButton.triggers.Clear();
    }

    private void ConfigurePlayerStatsPanel()
    {
        m_playerStatsPanelOpenPosition = m_playerStatsPanel.anchoredPosition;
        m_playerStatsPanelClosedPosition = -m_playerStatsPanelOpenPosition;

        m_playerStatsPanel.anchoredPosition = m_playerStatsPanelClosedPosition;
    }

    private void ConfigurePlayerInventoryPanel()
    {
        m_playerInventoryPanelOpenPosition = m_playerInventoryPanel.anchoredPosition;
        m_playerInventoryPanelClosedPosition = -m_playerInventoryPanelOpenPosition;

        m_playerInventoryPanel.anchoredPosition = m_playerInventoryPanelClosedPosition;
    }

    private void ConfigurePlayerItemInfoPanel()
    {
        m_playerItemInfoPanelOpenPosition = m_playerItemInfoPanel.anchoredPosition;
        m_playerItemInfoPanelClosedPosition = -m_playerItemInfoPanelOpenPosition;

        m_playerItemInfoPanel.anchoredPosition = m_playerItemInfoPanelClosedPosition;
    }

    private void ShowPlayerPanel(RectTransform playerPanel, RectTransform closePanel, Vector2 panelOpenPosition)
    {
        playerPanel.gameObject.SetActive(true);
        closePanel.gameObject.SetActive(true);
        closePanel.GetComponent<Image>().raycastTarget = true;

        LeanTween.cancel(playerPanel);
        LeanTween.move(playerPanel, panelOpenPosition, 0.5f).setEase(LeanTweenType.easeInCubic);


        LeanTween.cancel(closePanel);
        LeanTween.alpha(closePanel, .8f, 0.5f).setRecursive(false);

    }

    private void HidePlayerPanel(RectTransform playerPanel, RectTransform closePanel, Vector2 panelClosePosition)
    {
        closePanel.GetComponent<Image>().raycastTarget = false;

        LeanTween.cancel(playerPanel);
        LeanTween.move(playerPanel, panelClosePosition, 0.5f)
        .setEase(LeanTweenType.easeInCubic)
        .setOnComplete(() => { playerPanel.gameObject.SetActive(false); });


        LeanTween.cancel(closePanel);
        LeanTween.alpha(closePanel, .0f, 0.5f)
        .setRecursive(false)
        .setOnComplete(() => { closePanel.gameObject.SetActive(false); });
    }
}
