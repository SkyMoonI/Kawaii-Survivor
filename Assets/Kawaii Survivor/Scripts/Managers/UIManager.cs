using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour, IGameStateListener
{
    [Header("Panels")]

    [SerializeField] private GameObject m_menuPanel; // Reference to the menu panel
    [SerializeField] private GameObject m_weaponSelectionPanel; // Reference to the weapon selection panel
    [SerializeField] private GameObject m_gamePanel; // Reference to the game panel
    [SerializeField] private GameObject m_gameOverPanel; // Reference to the game over panel
    [SerializeField] private GameObject m_stageCompletePanel; // Reference to the stage complete panel
    [SerializeField] private GameObject m_waveTransitionPanel; // Reference to the wave transition panel
    [SerializeField] private GameObject m_shopPanel; // Reference to the shop panel


    private List<GameObject> m_panels = new List<GameObject>(); // List to hold all panels

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
}