using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameState CurrentGameState { get; private set; } // Property to store the current game state

    [Header("Elements")]
    [field: SerializeField] public bool IsUsingInfiniteMap { get; private set; }


    [Header("Actions")]
    public static Action onGamePaused;
    public static Action onGameResumed;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
        }
    }
    void Start()
    {
        Application.targetFrameRate = 60;
        SetGameState(GameState.MENU); // Set the initial game state to menu
    }

    public void SetGameState(GameState gameState)
    {
        IEnumerable<IGameStateListener> gameStateListeners =
        FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
        .OfType<IGameStateListener>(); // Find all game state listeners in the scene

        foreach (IGameStateListener listener in gameStateListeners)
        {
            listener.GameStateChangedCallBack(gameState); // Notify each listener of the state change
        }

        CurrentGameState = gameState; // Update the current game state
    }

    public void WaveCompletedCallBack()
    {
        if (Player.Instance.HasLevelUp() || WaveTransitionManager.Instance.HasChestCollected())
        {
            SetGameState(GameState.WAVETRANSITION); // Set the game state to wave transition
        }
        else
        {
            SetGameState(GameState.SHOP); // Set the game state to game
        }
    }

    public void StartGame() => SetGameState(GameState.GAME); // Start the game by setting the state to game
    public void StartWeaponSelection() => SetGameState(GameState.WEAPONSELECTION); // Start the game by setting the state to game
    public void OpenShop() => SetGameState(GameState.SHOP); // Open the shop by setting the state to shop

    public void ManageGameOver()
    {
        SceneManager.LoadScene(0); // Reload the current scene
    }

    public void PauseGame()
    {
        Time.timeScale = 0f; // Pause the game

        onGamePaused?.Invoke();
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f; // Resume the game

        onGameResumed?.Invoke();
    }

    public void RestartFromPause()
    {
        Time.timeScale = 1f; // Resume the game

        ManageGameOver();
    }
}

public interface IGameStateListener
{
    void GameStateChangedCallBack(GameState gameState); // Method to be called when the game state changes
}