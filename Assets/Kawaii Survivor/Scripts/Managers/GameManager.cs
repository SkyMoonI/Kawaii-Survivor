using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

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
    }

    public void ManageGameOver()
    {
        SceneManager.LoadScene(0); // Reload the current scene
    }

    public void WaveCompletedCallBack()
    {
        if (Player.Instance.HasLevelUp())
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
}

public interface IGameStateListener
{
    void GameStateChangedCallBack(GameState gameState); // Method to be called when the game state changes
}