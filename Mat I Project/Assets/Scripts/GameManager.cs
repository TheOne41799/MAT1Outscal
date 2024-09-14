using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance {  get { return instance; } }

    [SerializeField] private GameObject snakePrefab;

    [SerializeField] private Transform spawnPointPlayer1;
    [SerializeField] private Transform spawnPointPlayer2;

    [SerializeField] private GameMode currentGameMode = GameMode.ONE_PLAYER;
    public GameMode CurrentGameMode { get { return currentGameMode; } }

    [SerializeField] private GameState currentGameState = GameState.MAIN_MENU;
    public GameState CurrentGameState { get { return currentGameState; } }

    [SerializeField] private float moveInterval = 0.1f;

    private List<GameObject> activeSnakes = new List<GameObject>();

    [SerializeField] private FoodSpawningController foodSpawningController;
    [SerializeField] private PowerupSpawningController powerupSpawningController;

    private bool hasGameStarted = false;
    private bool hasSpawningStarted = false;

    private GameState previousGameMenuState;
    private YesNoPromptContext nextGameMenuContext;

    private bool hasBackgroundMusicStarted = false;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    private void Start()
    {
        SetGameState(GameState.MAIN_MENU);
    }


    public void SetGameMode(GameMode gameMode)
    {
        currentGameMode = gameMode;
    }


    public void SetGameState(GameState newState)
    {
        currentGameState = newState;

        switch (currentGameState)
        {
            case GameState.MAIN_MENU:
                SetTimeScaleToZero();

                hasGameStarted = false;
                hasSpawningStarted = false;
                StopSpawning();

                nextGameMenuContext = YesNoPromptContext.NONE;
                                
                SoundManager.Instance.TurnONBackgroundMusic(false);
                hasBackgroundMusicStarted = SoundManager.Instance.IsBackgroundMUsicON;
                SoundManager.Instance.TurnONSoundsSFX(true);

                UIManager.Instance.DeactivateAllPowerups();
                UIManager.Instance.ShowMainMenu();
                break;

            case GameState.PAUSE_MENU:
                SetTimeScaleToZero();

                UIManager.Instance.ShowPauseMenu();                
                break;

            case GameState.RESTART_MENU_ONE_PLAYER:
                SetTimeScaleToZero();

                SoundManager.Instance.TurnONBackgroundMusic(false);
                hasBackgroundMusicStarted = SoundManager.Instance.IsBackgroundMUsicON;

                hasGameStarted = false;
                hasSpawningStarted = false;
                StopSpawning();

                UIManager.Instance.DeactivateAllPowerups();
                UIManager.Instance.ShowRestartMenuOnePlayer();
                break;

            case GameState.RESTART_MENU_TWO_PLAYER:
                SetTimeScaleToZero();

                SoundManager.Instance.TurnONBackgroundMusic(false);
                hasBackgroundMusicStarted = SoundManager.Instance.IsBackgroundMUsicON;

                hasGameStarted = false;
                hasSpawningStarted = false;
                StopSpawning();

                UIManager.Instance.ShowRestartMenuTwoPlayer();
                break;

            case GameState.YES_NO_PROMPT:
                UIManager.Instance.ShowYesNoPromptMenu();
                break;

            case GameState.IN_GAME_UI_ONE_PLAYER:                
                SetTimeScaleToOne();

                SoundManager.Instance.TurnONBackgroundMusic(true);
                if(!hasBackgroundMusicStarted) SoundManager.Instance.PlayBackgroundMusic(Sounds.BACKGROUND_MUSIC);
                hasBackgroundMusicStarted = SoundManager.Instance.IsBackgroundMUsicON;

                if (!hasGameStarted)
                {
                    SetGameMode(GameMode.ONE_PLAYER);
                    StartGame();
                    hasGameStarted = true;
                    CleanUpEverythingFromPreviousGameplay();
                }

                if (!hasSpawningStarted)
                {
                    foodSpawningController.SpawnRandomFood();
                    powerupSpawningController.StartSpawningPowerups();
                }

                hasSpawningStarted = true;

                UIManager.Instance.ShowInGameUIOnePlayer();
                break;

            case GameState.IN_GAME_UI_TWO_PLAYER:
                SetTimeScaleToOne();

                SoundManager.Instance.TurnONBackgroundMusic(true);
                if (!hasBackgroundMusicStarted) SoundManager.Instance.PlayBackgroundMusic(Sounds.BACKGROUND_MUSIC);
                hasBackgroundMusicStarted = SoundManager.Instance.IsBackgroundMUsicON;

                if (!hasGameStarted)
                {
                    SetGameMode(GameMode.TWO_PLAYER);
                    StartGame();
                    hasGameStarted = true;
                    CleanUpEverythingFromPreviousGameplay();
                }

                if (!hasSpawningStarted)
                {
                    foodSpawningController.SpawnRandomFood();
                    powerupSpawningController.StartSpawningPowerups();
                }

                hasSpawningStarted = true;

                UIManager.Instance.ShowInGameUITwoPlayer();
                break;
        }
    }


    private void StartGame()
    {
        ClearExistingSnakes();

        switch (currentGameMode)
        {
            case GameMode.ONE_PLAYER:
                SpawnSnake(spawnPointPlayer1.position, moveInterval, 1); 
                break;
            case GameMode.TWO_PLAYER:
                SpawnSnake(spawnPointPlayer1.position, moveInterval, 1);
                SpawnSnake(spawnPointPlayer2.position, moveInterval, 2);
                break;
        }
    }


    private void ClearExistingSnakes()
    {
        foreach(var snake in activeSnakes)
        {
            snake.GetComponent<PlayerController>().RemoveAllSegments();
            Destroy(snake);
        }

        activeSnakes.Clear();
    }


    private void SpawnSnake(Vector2 spawnPosition, float speed, int playerID)
    {
        GameObject newSnake = Instantiate(snakePrefab, spawnPosition, Quaternion.identity);

        PlayerController playerController = newSnake.GetComponent<PlayerController>();
        playerController.playerID = playerID;
        playerController.InitializeSnake(speed, spawnPosition);


        if (playerID == 1)
        {
            newSnake.tag = "PlayerOne";
        }
        else if (playerID == 2)
        {
            newSnake.tag = "PlayerTwo";
        }

        activeSnakes.Add(newSnake);
    }



    public void GameOverOnePlayerMode()
    {
        SoundManager.Instance.Play(Sounds.GAME_OVER);
        SetGameState(GameState.RESTART_MENU_ONE_PLAYER);
    }

    public void PlayerOneWinsInTwoPlayerMode()
    {
        SoundManager.Instance.Play(Sounds.GAME_OVER);
        SetGameState(GameState.RESTART_MENU_TWO_PLAYER);
        UIManager.Instance.ShowPlayerOneWins();
    }


    public void PlayerTwoWinsInTwoPlayerMode()
    {
        SoundManager.Instance.Play(Sounds.GAME_OVER);
        SetGameState(GameState.RESTART_MENU_TWO_PLAYER);
        UIManager.Instance.ShowPlayerTwoWins();
    }


    private void SetTimeScaleToZero()
    {
        Time.timeScale = 0; 
    }


    private void SetTimeScaleToOne()
    {
        Time.timeScale = 1;
    }


    public void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }


    public void StartOnePlayerGame()
    {
        SetGameState(GameState.IN_GAME_UI_ONE_PLAYER);
    }


    public void StartTwoPlayerGame()
    {
        SetGameState(GameState.IN_GAME_UI_TWO_PLAYER);
    }


    public void PauseGame()
    {
        SetGameState(GameState.PAUSE_MENU);
    }


    public void ResumeGame()
    {
        SetTimeScaleToOne();

        if (currentGameMode == GameMode.ONE_PLAYER)
        {            
            SetGameState(GameState.IN_GAME_UI_ONE_PLAYER);
        }
        else if (currentGameMode == GameMode.TWO_PLAYER)
        {
            SetGameState(GameState.IN_GAME_UI_TWO_PLAYER);
        }
    }


    public void RestartGame()
    {
        if (currentGameMode == GameMode.ONE_PLAYER)
        {
            SetGameState(GameState.IN_GAME_UI_ONE_PLAYER);
        }
        else if (currentGameMode == GameMode.TWO_PLAYER)
        {
            SetGameState(GameState.IN_GAME_UI_TWO_PLAYER);
        }
    }


    private void StopSpawning()
    {
        foodSpawningController.StopAllCoroutines();
        powerupSpawningController.StopAllCoroutines();
    }


    private void CleanUpEverythingFromPreviousGameplay()
    {
        foodSpawningController.DestroyCurrentFood();
        powerupSpawningController.DestroyPowerupsOnGameRestart();
    }


    public void OnMainMenuButtonClicked()
    {
        nextGameMenuContext = YesNoPromptContext.MAIN_MENU;
        OpenYesNoPrompt();
    }


    public void OnQuitButtonClicked()
    {
        nextGameMenuContext = YesNoPromptContext.QUIT_GAME;
        OpenYesNoPrompt();
    }


    private void OpenYesNoPrompt()
    {
        previousGameMenuState = currentGameState;

        SetGameState(GameState.YES_NO_PROMPT);
    }


    public void YesButtonPressedOnYesNoPromptMenu()
    {  
        switch(nextGameMenuContext)
        {
            case YesNoPromptContext.QUIT_GAME:
                QuitGame(); 
                break;
            case YesNoPromptContext.MAIN_MENU:
                SetGameState(GameState.MAIN_MENU);
                break;
        }
        
    }


    public void NoButtonPressedOnYesNoPromptMenu()
    {
        SetGameState(previousGameMenuState);
    }
}


public enum GameMode
{
    ONE_PLAYER,
    TWO_PLAYER,
    NONE
}


public enum GameState
{
    MAIN_MENU,
    PAUSE_MENU,
    RESTART_MENU_ONE_PLAYER,
    RESTART_MENU_TWO_PLAYER,
    YES_NO_PROMPT,
    IN_GAME_UI_ONE_PLAYER, 
    IN_GAME_UI_TWO_PLAYER,
    NONE
}


public enum YesNoPromptContext
{
    QUIT_GAME,
    MAIN_MENU,
    NONE
}







