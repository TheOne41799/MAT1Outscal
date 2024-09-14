using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    public static UIManager Instance { get { return instance; } }

    [SerializeField] private GameObject titleTextGameObject;
    [SerializeField] private GameObject mainMenuUIGameObject;
    [SerializeField] private GameObject pauseMenuUIGameObject;
    [SerializeField] private GameObject restartMenuOnePlayerModeGameObject;
    [SerializeField] private GameObject restartMenuTwoPlayerModeGameObject;
    [SerializeField] private GameObject yesNoPromptGameObject;
    [SerializeField] private GameObject inGameUIOnePlayerModeGameObject;
    [SerializeField] private GameObject inGameUITwoPlayerModeGameObject;

    [SerializeField] private GameObject playerOneWinsText;
    [SerializeField] private GameObject playerTwoWinsText;

    //private YesNoPromptContext currentPromptContext;
    //public GameState previousGameMenu;


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


    public void ShowMainMenu()
    {
        HideAllUI();
        mainMenuUIGameObject.SetActive(true);
        titleTextGameObject.SetActive(true);
    }

    public void ShowPauseMenu()
    {
        HideAllUI();
        pauseMenuUIGameObject.SetActive(true);
    }

    public void ShowRestartMenuOnePlayer()
    {
        HideAllUI();
        restartMenuOnePlayerModeGameObject.SetActive(true);
    }

    public void ShowRestartMenuTwoPlayer()
    {
        HideAllUI();
        restartMenuTwoPlayerModeGameObject.SetActive(true);
    }

    


    public void ShowInGameUIOnePlayer()
    {
        HideAllUI();
        inGameUIOnePlayerModeGameObject.SetActive(true);
    }

    public void ShowInGameUITwoPlayer()
    {
        HideAllUI();
        inGameUITwoPlayerModeGameObject.SetActive(true);
    }

    public void ShowYesNoPromptMenu()
    {
        HideAllUI();
        yesNoPromptGameObject.SetActive(true);
    }

    public void ShowPlayerOneWins()
    {
        playerOneWinsText.SetActive(true);
    }

    public void ShowPlayerTwoWins()
    {
        playerTwoWinsText.SetActive(true);
    }

    private void HideAllUI()
    {
        titleTextGameObject.SetActive(false);
        mainMenuUIGameObject.SetActive(false);
        pauseMenuUIGameObject.SetActive(false);
        restartMenuOnePlayerModeGameObject.SetActive(false);
        restartMenuTwoPlayerModeGameObject.SetActive(false);
        yesNoPromptGameObject.SetActive(false);
        inGameUIOnePlayerModeGameObject.SetActive(false);
        inGameUITwoPlayerModeGameObject.SetActive(false);
        playerOneWinsText.SetActive(false);
        playerTwoWinsText.SetActive(false);
    }










    /*public void ShowYesNoPrompt(YesNoPromptContext context, GameState previousMenu)
    {
        HideAllUI();

        this.currentPromptContext = context;
        this.previousGameMenu = previousMenu;
        yesNoPromptGameObject.SetActive(true);
    }

    public void OnYesButtonClicked()
    {
        switch (currentPromptContext)
        {
            case YesNoPromptContext.QUIT_GAME:
                GameManager.Instance.QuitGame();
                break;
        }
    }


    public void OnNoButtonClicked()
    {
        yesNoPromptGameObject.SetActive(false);

        switch (previousGameMenu)
        {
            case GameState.MAIN_MENU:
                ShowMainMenu();
                break;

        }
    }*/
}


/*public enum YesNoPromptContext
{
    QUIT_GAME,
    RETURN_TO_MAIN_MENU_FROM_PAUSE,
    RETURN_TO_MAIN_MENU_FROM_RESTART_ONE_PLAYER,
    RETURN_TO_MAIN_MENU_FROM_RESTART_TWO_PLAYER,
    RETURN_TO_MAIN_MENU_FROM_QUIT,
    RETURN_TO_PAUSE_MENU_FROM_MAIN_MENU,
    RETURN_TO_PAUSE_MENU_FROM_QUIT,
    RETURN_TO_PAUSE_FROM_QUIT,
    RETURN_TO_RESTART_ONE_PLAYER_FROM_MAIN_MENU,
    RETURN_TO_RESTART_ONE_PLAYER_FROM_QUIT,
    RETURN_TO_RESTART_TWO_PLAYER_FROM_MAIN_MENU,
    RETURN_TO_RESTART_TWO_PLAYER_FROM_QUIT
}*/



























/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    public static UIManager Instance { get { return instance; } }

    [SerializeField] private GameObject titleTextGameObject;
    [SerializeField] private GameObject mainMenuUIGameObject;
    [SerializeField] private GameObject pauseMenuUIGameObject;
    [SerializeField] private GameObject restartMenuOnePlayerModeGameObject;
    [SerializeField] private GameObject restartMenuTwoPlayerModeGameObject;
    [SerializeField] private GameObject yesNoPromptGameObject;
    [SerializeField] private GameObject inGameUIOnePlayerModeGameObject;
    [SerializeField] private GameObject inGameUITwoPlayerModeGameObject;

    [SerializeField] private GameObject playerOneWinsText;
    [SerializeField] private GameObject playerTwoWinsText;
    public int whichPlayerWon;

    //private GameObject previousGameObject;

    private Dictionary<GameState, GameObject> uiElements = new Dictionary<GameState, GameObject>();

    //private int gameStateInt = 0;
    //public int GameStateInt { set { gameStateInt = value; } get { return gameStateInt; } }*/


/*private void Awake()
{
    if(instance == null)
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
    uiElements[GameState.MAIN_MENU] = mainMenuUIGameObject;
    uiElements[GameState.PAUSE_MENU] = pauseMenuUIGameObject;
    uiElements[GameState.RESTART_MENU_ONE_PLAYER] = restartMenuOnePlayerModeGameObject;
    uiElements[GameState.RESTART_MENU_TWO_PLAYER] = restartMenuTwoPlayerModeGameObject;
    uiElements[GameState.YES_NO_PROMPT] = yesNoPromptGameObject;
    uiElements[GameState.IN_GAME_UI_ONE_PLAYER] = inGameUIOnePlayerModeGameObject;
    uiElements[GameState.IN_GAME_UI_TWO_PLAYER] = inGameUITwoPlayerModeGameObject;

    UpdateUIState(GameManager.Instance.CurrentGameState);
    ShowTitleTextGO();


}


public void UpdateUIState(int newGameStateInt)
{
    HideAllUIElements();

    GameState newState = (GameState)newGameStateInt; 

    if (uiElements.ContainsKey(newState))
    {
        ShowUIElement(uiElements[newState]);
    }

    Debug.Log(newState);

    GameManager.Instance.SetGameStateEnumToGameStateInt(newGameStateInt);

    ShowHideTitleText();

    GameManager.Instance.CurrentGameState = newState;

    GameManager.Instance.ChangeGameState();
}


public void UpdateUIState(GameState newState)
{
    HideAllUIElements();

    if (uiElements.ContainsKey(newState))
    {
        ShowUIElement(uiElements[newState]);
    }

    Debug.Log(newState);

    GameManager.Instance.SetGameStateIntToGameStateEnum(newState);

    ShowHideTitleText();

    GameManager.Instance.CurrentGameState = newState;

    GameManager.Instance.ChangeGameState();
}


private void HideAllUIElements()
{
    foreach (var uiElement in uiElements.Values)
    {
        HideUIElement(uiElement);
    }
}


private void HideUIElement(GameObject uiElement)
{
    uiElement.SetActive(false);
}


private void ShowUIElement(GameObject uiElement)
{
    uiElement.SetActive(true);
}




private void ShowHideTitleText()
{
    if (GameManager.Instance.CurrentGameState != GameState.MAIN_MENU)
    {
        HideTitleTextGO();
    }
    else
    {
        ShowTitleTextGO();
    }
}


public void ResumeGame()
{
    GameManager.Instance.ResumeGame(); 
}



public void QuitGame()
{
    GameManager.Instance.QuitGame();
}


public void YesNoPrompt()
{

}


public void WhichPlayerWon()
{
    if(whichPlayerWon == 1)
    {
        playerOneWinsText.SetActive(true);
        playerTwoWinsText.SetActive(false);
    }
    else if(whichPlayerWon == 2)
    {
        playerOneWinsText.SetActive(false);
        playerTwoWinsText.SetActive(true);
    }
}





*//*public void UpdateUIState(GameState newState)
{
    HideAllUIElements();

    if (uiElements.ContainsKey(newState))
    {
        ShowUIElement(uiElements[newState]);
    }

    Debug.Log(newState);
}


private void HideAllUIElements()
{
    foreach (var uiElement in uiElements.Values)
    {
        HideUIElement(uiElement);
    }
}


private void HideUIElement(GameObject uiElement)
{
    uiElement.SetActive(false);
}


private void ShowUIElement(GameObject uiElement)
{
    uiElement.SetActive(true);
}*/


/*public void ChangeGameState(int gameStateInt)
{

}*//*


public void ShowTitleTextGO() => titleTextGameObject.SetActive(true);
public void HideTitleTextGO() => titleTextGameObject.SetActive(false);





*//*//public void ShowMainMenuGO() => mainMenuUIGameObject.SetActive(true);
*//*private void HideMainMenuGO() => mainMenuUIGameObject.SetActive(false);
private void ShowPauseMenuGO() => pauseMenuUIGameObject .SetActive(true);
private void HidePauseMenuGO() => pauseMenuUIGameObject.SetActive(false);
private void ShowRestartMenuOnePlayerModeGO() => restartMenuOnePlayerModeGameObject.SetActive(true);
private void HideRestartMenuOnePlayerModeGO() => restartMenuOnePlayerModeGameObject .SetActive(false);
private void ShowRestartMenuTwoPlayerModeGO() => restartMenuTwoPlayerModeGameObject.SetActive(true);
private void HideRestartMenuTwoPlayerModeGO() => restartMenuTwoPlayerModeGameObject.SetActive(false);
private void ShowYesNOPromptGO() => yesNoPromptGameObject.SetActive(true);
private void HideYesNoPromptGO() => yesNoPromptGameObject.SetActive(false);
private void ShowInGameUIOnePlayerModeGO() => inGameUIOnePlayerModeGameObject.SetActive(true);
private void HideInGameUIOnePlayerModeGO() => inGameUIOnePlayerModeGameObject.SetActive(false);
private void ShowInGameUITwoPlayerModeGO() => inGameUITwoPlayerModeGameObject.SetActive(true);
private void HideInGameUITwoPlayerModeGO() => inGameUITwoPlayerModeGameObject.SetActive(false);*//*
}









*/