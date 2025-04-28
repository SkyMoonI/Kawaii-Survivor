using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour, IGameStateListener
{
    [Header("Panels")]
    [SerializeField] private GameObject m_menuPanel; // Reference to the menu panel
    [SerializeField] private GameObject m_weaponSelectionPanel; // Reference to the weapon selection panel
    [SerializeField] private GameObject m_gamePanel; // Reference to the game panel
    [SerializeField] private GameObject m_pausePanel; // Reference to the game panel
    [SerializeField] private GameObject m_restartConfirmationPanel; // Reference to the game panel
    [SerializeField] private GameObject m_gameOverPanel; // Reference to the game over panel
    [SerializeField] private GameObject m_stageCompletePanel; // Reference to the stage complete panel
    [SerializeField] private GameObject m_waveTransitionPanel; // Reference to the wave transition panel
    [SerializeField] private GameObject m_shopPanel; // Reference to the shop panel

    [Header("Settings")]
    private List<GameObject> m_panels = new List<GameObject>(); // List to hold all panels

    [Header("Buttons")]
    [SerializeField] private Button m_pauseButton;
    [SerializeField] private Button m_resumeButton;
    [SerializeField] private Button m_openRestartConfirmationPanel;
    [SerializeField] private Button m_restartConfirmationPanelYesButton;
    [SerializeField] private Button m_restartConfirmationPanelNoButton;
    [SerializeField] private Button m_openMenuButton;


    void Awake()
    {
        m_panels.AddRange(new GameObject[]
        {
            m_menuPanel,
            m_weaponSelectionPanel,
            m_gamePanel,
            m_gameOverPanel,
            m_stageCompletePanel,
            m_waveTransitionPanel,
            m_shopPanel,
        }); // Add all panels to the list
    }

    void Start()
    {
        m_pausePanel.SetActive(false);
        HideRestartConfirmationPanel();
    }

    void OnEnable()
    {
        GameManager.onGamePaused += GamePausedCallBack; // Subscribe to the game paused event
        GameManager.onGameResumed += GameResumedCallBack; // Subscribe to the game resumed event

        // Delay the call by 2 frames to ensure that the game manager is instanced
        LeanTween.delayedCall(Time.deltaTime * 2, () =>
        {
            m_pauseButton.onClick.RemoveAllListeners();
            m_pauseButton.onClick.AddListener(GameManager.Instance.PauseGame);

            m_resumeButton.onClick.RemoveAllListeners();
            m_resumeButton.onClick.AddListener(GameManager.Instance.ResumeGame);

            m_restartConfirmationPanelYesButton.onClick.RemoveAllListeners();
            m_restartConfirmationPanelYesButton.onClick.AddListener(GameManager.Instance.RestartFromPause);

            m_openMenuButton.onClick.RemoveAllListeners();
            m_openMenuButton.onClick.AddListener(GameManager.Instance.ManageGameOver);
        });

        m_openRestartConfirmationPanel.onClick.RemoveAllListeners(); // Remove all listeners from the button
        m_openRestartConfirmationPanel.onClick.AddListener(ShowRestartConfirmationPanel); // Add a new listener to handle button clicks

        m_restartConfirmationPanelNoButton.onClick.RemoveAllListeners(); // Remove all listeners from the button
        m_restartConfirmationPanelNoButton.onClick.AddListener(HideRestartConfirmationPanel); // Add a new listener to handle button clicks
    }

    void OnDisable()
    {
        GameManager.onGamePaused -= GamePausedCallBack; // Unsubscribe from the game paused event
        GameManager.onGameResumed -= GameResumedCallBack; // Unsubscribe from the game resumed event

        m_pauseButton.onClick.RemoveAllListeners();
        m_resumeButton.onClick.RemoveAllListeners();
        m_openRestartConfirmationPanel.onClick.RemoveAllListeners();
        m_openMenuButton.onClick.RemoveAllListeners();
    }

    void OnDestroy()
    {
        GameManager.onGamePaused -= GamePausedCallBack; // Unsubscribe from the game paused event
        GameManager.onGameResumed -= GameResumedCallBack; // Unsubscribe from the game resumed event

        m_pauseButton.onClick.RemoveAllListeners();
        m_resumeButton.onClick.RemoveAllListeners();
        m_openRestartConfirmationPanel.onClick.RemoveAllListeners();
        m_openMenuButton.onClick.RemoveAllListeners();
    }

    public void GameStateChangedCallBack(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.MENU:
                ShowPanel(m_menuPanel); // Show the menu panel
                break;
            case GameState.WEAPONSELECTION:
                ShowPanel(m_weaponSelectionPanel); // Show the weapon selection panel
                break;
            case GameState.GAME:
                ShowPanel(m_gamePanel); // Show the game panel
                break;
            case GameState.GAMEOVER:
                ShowPanel(m_gameOverPanel); // Show the game over panel
                break;
            case GameState.STAGECOMPLETE:
                ShowPanel(m_stageCompletePanel); // Show the stage complete panel
                break;
            case GameState.WAVETRANSITION:
                ShowPanel(m_waveTransitionPanel); // Show the wave transition panel
                break;
            case GameState.SHOP:
                ShowPanel(m_shopPanel); // Show the shop panel
                break;
            default:
                break;
        }
    }

    private void ShowPanel(GameObject panel)
    {
        foreach (GameObject p in m_panels)
        {
            p.SetActive(p == panel); // Activate the selected panel and deactivate others
        }
    }

    private void GameResumedCallBack()
    {
        m_pausePanel.SetActive(false);
    }

    private void GamePausedCallBack()
    {
        m_pausePanel.SetActive(true);
    }

    private void ShowRestartConfirmationPanel()
    {
        m_restartConfirmationPanel.SetActive(true);
    }

    private void HideRestartConfirmationPanel()
    {
        m_restartConfirmationPanel.SetActive(false);
    }
}