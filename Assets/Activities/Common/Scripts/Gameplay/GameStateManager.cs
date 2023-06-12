using System;
using UnityEngine.SceneManagement;
using static GameStateManager.GameState;

public static class GameStateManager
{
    [Serializable]
    public enum GameState
    {
        Playing,
        Menu,
        Pause,
        Testing,
        MainMenu
    }
    
    private static GameState _state = Menu; //Todo: may need to set as MainMenu.
    public static GameState State
    {
        get => _state;
        private set
        {
            //This way when the event is called _state is still previous value
            GameStateChanged?.Invoke(value);
            _state = value;
        }
    }

    public static bool IsPlaying => State == Playing;
    
    public static event Action<GameState> GameStateChanged;
    public static event Action GameStarted;
    public static event Action GameEnded;
    public static event Action ReturnToMainMenu;
    
    public static void StartGame()
    {
        State = Playing;
        GameStarted?.Invoke();
    }

    public static void EndGame()
    {
        State = Menu;
        GameEnded?.Invoke();
    }

    public static void LoadMainMenu()
    {
        State = MainMenu;
        ReturnToMainMenu?.Invoke();
        SceneManager.LoadScene("Menu");
    }

    // Not used yet
    public static void PauseGame(bool pause)
    {
        State = pause ? Pause : Playing;
    }

    public static void TestingGame(bool testing)
    {
        State = testing ? Testing : Playing;
    }
}
