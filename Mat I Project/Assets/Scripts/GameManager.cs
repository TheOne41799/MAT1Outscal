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
        //StartGame();
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
                nextGameMenuContext = YesNoPromptContext.NONE;
                StopSpawning();
                UIManager.Instance.ShowMainMenu();
                break;
            case GameState.PAUSE_MENU:
                SetTimeScaleToZero();
                UIManager.Instance.ShowPauseMenu();                
                break;
            case GameState.RESTART_MENU_ONE_PLAYER:
                SetTimeScaleToZero();
                hasGameStarted = false;
                hasSpawningStarted = false;
                StopSpawning();
                UIManager.Instance.ShowRestartMenuOnePlayer();
                break;
            case GameState.RESTART_MENU_TWO_PLAYER:
                SetTimeScaleToZero();
                hasGameStarted = false;
                hasSpawningStarted = false;
                StopSpawning();
                UIManager.Instance.ShowRestartMenuTwoPlayer();
                break;
            case GameState.YES_NO_PROMPT:
                //UIManager.Instance.ShowYesNoPrompt();
                //OpenYesNoPrompt();
                //Debug.Log("YesNo");
                UIManager.Instance.ShowYesNoPromptMenu();






                break;
            case GameState.IN_GAME_UI_ONE_PLAYER:
                SetTimeScaleToOne();
                /*if (!hasGameStarted) SetGameMode(GameMode.ONE_PLAYER);
                if (!hasGameStarted) StartGame();
                hasGameStarted = true;*/
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
                if (!hasGameStarted)
                {
                    SetGameMode(GameMode.TWO_PLAYER);
                    StartGame();
                    hasGameStarted = true;
                    CleanUpEverythingFromPreviousGameplay();
                }



                /*if (!hasGameStarted) SetGameMode(GameMode.TWO_PLAYER);
                if (!hasGameStarted) StartGame();*/
                //hasGameStarted = true;
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
        Debug.Log("Game over");
        SetGameState(GameState.RESTART_MENU_ONE_PLAYER);
    }

    public void PlayerOneWinsInTwoPlayerMode()
    {
        Debug.Log("Player One Wins");

        

        SetGameState(GameState.RESTART_MENU_TWO_PLAYER);
        UIManager.Instance.ShowPlayerOneWins();
    }


    public void PlayerTwoWinsInTwoPlayerMode()
    {
        Debug.Log("Player Two Wins");

        

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

    /*public void GoToMainMenu()
    {
        SetGameState(GameState.MAIN_MENU);
    }*/

    public void RestartGame()
    {
        // Restart based on current game mode (One or Two Player)
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
        //GameState previousGameState = currentGameState;
        //UIManager.Instance.previousGameMenu = currentGameState;
        previousGameMenuState = currentGameState;


        //Debug.Log(previousGameState);
        SetGameState(GameState.YES_NO_PROMPT);
    }


    public void YesButtonPressedOnYesNoPromptMenu()
    {
        Debug.Log("Confirmed!");
        
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
        Debug.Log("Cancelled");
        //SetGameState(GameState.PAUSE_MENU);

        SetGameState(previousGameMenuState);

        /*switch (previousGameMenuState)
        {
            case GameState.MAIN_MENU:
                SetGameState(GameState.MAIN_MENU);
                break;
        }*/
    }







    /*public void OpenYesNoPrompt()
    {
        SetGameState(GameState.YES_NO_PROMPT);
    }


    public void ShowPrompt(YesNoPromptContext context, GameState previousMenu)
    {
        SetGameState(GameState.YES_NO_PROMPT);
        UIManager.Instance.ShowYesNoPrompt(context, previousMenu);
    }


    public void ConfirmAction()
    {
        Debug.Log("Confirmed!");
        //SetGameState(GameState.MAIN_MENU);
    }

    public void CancelAction()
    {
        Debug.Log("Cancelled");
        //SetGameState(GameState.PAUSE_MENU);
    }*/








    /*public void OnQuitButtonClicked()
    {
        OpenYesNoPrompt();
    }

    public void OnMainMenuButtonClicked()
    {
        OpenYesNoPrompt();
    }*/
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







/*


using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }


    [SerializeField] private GameObject snakePrefab;

    [SerializeField] private Transform spawnPointPlayer1;
    [SerializeField] private Transform spawnPointPlayer2;

    private GameMode currentGameMode;
    public GameMode CurrentGameMode { set { currentGameMode = value; } get { return currentGameMode; } }

    [SerializeField] private float moveInterval = 0.1f;

    private List<GameObject> activeSnakes = new List<GameObject>();

    private GameState currentGameState;
    public GameState CurrentGameState { set { currentGameState = value; } get { return currentGameState; } }

    private GameState previousState;
    public GameState PreviousState { set { previousState = value; } get { return previousState; } }

    private int gameStateInt = 0;
    public int GameStateInt { set { gameStateInt = value; } get { return gameStateInt; } }

    [SerializeField] private GameObject foodSpawner;
    [SerializeField] private GameObject powerupSpawner;

    //private bool areFoodsAndPowerupsSpawning = false;



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
        //UIManager.Instance.ShowTitleTextGO();
        //currentGameState = GameState.MAIN_MENU;
        //SetGameState(currentGameState);
        //
        //StartGame();

        //ChangeGameState(GameState.MAIN_MENU);

        //UIManager.Instance.ChangeGameState(1);
    }


    private void StartGame()
    {
        ClearExistingSnakes();

        SetTimeScaleToOne();

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

        //areFoodsAndPowerupsSpawning = true;
    }


    private void ClearExistingSnakes()
    {
        foreach (var snake in activeSnakes)
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


    *//*public void ChangeGameState(int gameStateInt)
    {

    }*/


/*public void ChangeGameState(GameState gameState)
{

    currentGameState = gameState;
    Debug.Log(currentGameState);


    switch (currentGameState)
    {
        case GameState.MAIN_MENU:
            UIManager.Instance.ShowMainMenuGO();
            //UIManager.Instance.UpdateUIState(currentGameState);
            break;
    }
}*/


/*public void ChangeGameMode(GameMode gameMode)
{
    currentGameMode = gameMode;
}*//*






public void SetGameStateEnumToGameStateInt(int gsInt)
{
    currentGameState = (GameState)gsInt;
}

public void SetGameStateIntToGameStateEnum(GameState state)
{
    gameStateInt = (int)state;
}


public void ChangeGameState()
{
    switch (currentGameState)
    {
        case GameState.MAIN_MENU:
            break;
        case GameState.IN_GAME_UI_ONE_PLAYER:
            currentGameMode = GameMode.ONE_PLAYER;
            if (previousState == GameState.MAIN_MENU) StartGame();
            //StopAllCoroutines();
            foodSpawner.GetComponent<FoodSpawningController>().SpawnRandomFood();
            powerupSpawner.GetComponent<PowerupSpawningController>().StartSpawningPowerups();
            ///areFoodsAndPowerupsSpawning = true;
            break;
        case GameState.IN_GAME_UI_TWO_PLAYER:
            currentGameMode = GameMode.TWO_PLAYER;
            if (previousState == GameState.MAIN_MENU) StartGame();
            foodSpawner.GetComponent<FoodSpawningController>().SpawnRandomFood();
            powerupSpawner.GetComponent<PowerupSpawningController>().StartSpawningPowerups();
            //areFoodsAndPowerupsSpawning = true;
            break;
        case GameState.PAUSE_MENU:
            SetTimeScaleToZero();
            break;
        case GameState.RESTART_MENU_ONE_PLAYER:
            StopAllCoroutines();
            powerupSpawner.GetComponent<PowerupSpawningController>().StopSpawningPowerups();

            break;
        case GameState.RESTART_MENU_TWO_PLAYER:

            break;


    }
}


public void GameOverOnePlayerMode()
{
    currentGameState = GameState.RESTART_MENU_ONE_PLAYER;
    UIManager.Instance.UpdateUIState(currentGameState);
    InputManager.Instance.ResetPlayerDirectionToDefault();
    //areFoodsAndPowerupsSpawning = false;
    SetTimeScaleToZero();
    Debug.Log("Game over");
}

public void PlayerOneWinsInTwoPlayerMode()
{
    currentGameState = GameState.RESTART_MENU_TWO_PLAYER;
    UIManager.Instance.whichPlayerWon = 1;
    UIManager.Instance.WhichPlayerWon();
    UIManager.Instance.UpdateUIState(currentGameState);
    InputManager.Instance.ResetPlayerDirectionToDefault();
    //areFoodsAndPowerupsSpawning = false;
    SetTimeScaleToZero();
    Debug.Log("Player One Wins");
}


public void PlayerTwoWinsInTwoPlayerMode()
{
    SetTimeScaleToZero();
    currentGameState = GameState.RESTART_MENU_TWO_PLAYER;
    UIManager.Instance.whichPlayerWon = 2;
    UIManager.Instance.WhichPlayerWon();
    UIManager.Instance.UpdateUIState(currentGameState);
    InputManager.Instance.ResetPlayerDirectionToDefault();
    //areFoodsAndPowerupsSpawning = false;
    Debug.Log("Player Two Wins");
}


public void ResumeGame()
{
    SetTimeScaleToOne();

    UIManager.Instance.UpdateUIState(previousState);
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










*//*public void SetGameState(GameState newGameState)
{
    currentGameState = newGameState;
}*/


/*public void SetGameState(GameState newGameState)
{
    currentGameState = newGameState;

    UIManager.Instance.UpdateUIState(newGameState);
}*/


/*public void SetGameMode(GameMode newGameMode)
{  
    currentGameMode = newGameMode; 
}*/


/*public void OnOnePlayerButtonClicked()
{
    SetGameMode(GameMode.ONE_PLAYER);
    SetGameState(GameState.IN_GAME_UI_ONE_PLAYER);
}


public void OnTwoPlayerButtonClicked()
{
    SetGameMode(GameMode.TWO_PLAYER);
    SetGameState(GameState.IN_GAME_UI_TWO_PLAYER);
}

public void OnPauseButtonClicked()
{
    SetGameState(GameState.PAUSE_MENU);
}*/


/*public void OnRestartButtonClicked()
{
    if (currentGameMode == GameMode.ONE_PLAYER)
    {
        SetGameState(GameState.IN_GAME_UI_ONE_PLAYER);
    }
    else
    {
        SetGameState(GameState.IN_GAME_UI_TWO_PLAYER);
    }
}*/


/*public void OnMainMenuButtonClicked()
{
    //SetGameState(GameState.YES_NO_PROMPT);
}*//*



}


public enum GameMode
{
ONE_PLAYER,
TWO_PLAYER
}


public enum GameState
{
MAIN_MENU,
PAUSE_MENU,
RESTART_MENU_ONE_PLAYER,
RESTART_MENU_TWO_PLAYER,
YES_NO_PROMPT,
IN_GAME_UI_ONE_PLAYER,
IN_GAME_UI_TWO_PLAYER
}





*/







