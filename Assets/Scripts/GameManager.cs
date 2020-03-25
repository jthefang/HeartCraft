using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum GameState {
    GAMEOVER,
    PAUSED,
    PLAYING,
    STARTING,
}

public class GameManager : MonoBehaviour, IDependentScript
{
    #region Singleton
    public static GameManager Instance;
    //reference this only version as MultiplierManager.Instance.SpawnSprite(randPos, UnityEngine.Random.rotation);
    private void Awake() {
        Instance = this;
    }
    #endregion

    #region GameState
    private GameState _currGameState;
    public GameState CurrGameState
    {
        get {
            return this._currGameState;
        }

        set {  
            GameState prevGameState = this._currGameState;
            this._currGameState = value;   
            if (prevGameState != value)   
                OnGameStateChange?.Invoke(prevGameState, value);
        }
    }
    public bool IsPlaying {
        get {
            return this.CurrGameState == GameState.PLAYING;
        }
    }
    public bool IsGameOver {
        get {
            return this.CurrGameState == GameState.GAMEOVER;
        }
        set {
            if (value)
                this.CurrGameState = GameState.GAMEOVER;
        }
    }
    public event Action<GameState, GameState> OnGameStateChange;
    public event Action<GameManager> OnGameStart;
    public event Action<GameManager> OnGameOver;
    public event Action<GameManager> OnGamePause;
    public event Action<GameManager> OnGameResume;
    #endregion
 
    // Start is called before the first frame update
    void Start()
    {
        OnGameStateChange += OnGameStateChangeHandler;
        OnGameStart += OnGameStartHandler;
        OnGameOver += OnGameOverHandler;
        OnGamePause += OnGamePauseHandler;
        OnGameResume += OnGameResumeHandler;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnAllDependenciesLoaded() {
        CurrGameState = GameState.STARTING;
    }

    void OnGameStateChangeHandler(GameState prevGameState, GameState newGameState) {
        if (newGameState == GameState.STARTING) {
            OnGameStart?.Invoke(this);
        } else if (newGameState == GameState.GAMEOVER) {
            OnGameOver?.Invoke(this);
        } else if (newGameState == GameState.PAUSED) {
            OnGamePause?.Invoke(this);
        } else if (newGameState == GameState.PLAYING) {
            if (prevGameState == GameState.PAUSED) {
                OnGameResume?.Invoke(this);
            }
        }
    }

    void OnGameStartHandler(GameManager gm) {
        CurrGameState = GameState.PLAYING;
    }

    void OnGameOverHandler(GameManager gm) {
        
    }

    void OnGamePauseHandler(GameManager gm) {
        
    }

    void OnGameResumeHandler(GameManager gm) {
        
    }

    public void StartGame() {
        CurrGameState = GameState.STARTING;
    }

    public void RestartGame() {
        CurrGameState = GameState.GAMEOVER;
        StartGame();
    }

}
