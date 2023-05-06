using System;
using static GameStateManager.GameState;

public static class GameStateManager
{
    [Serializable]
    public enum GameState
    {
        Playing,
        Menu,
        Pause
    }
    
    private static GameState _state;
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
    
    public static event Action<GameState> GameStateChanged;
    public static event Action GameStarted;
    public static event Action GameEnded;
    
    public static void StartExperiment()
    {
        State = Playing;
        GameStarted?.Invoke();
    }

    public static void EndExperiment()
    {
        State = Menu;
        GameEnded?.Invoke();
    }

    // Not used yet
    public static void PauseExperiment(bool pause)
    {
        State = pause ? Pause : Playing;
    }
}
