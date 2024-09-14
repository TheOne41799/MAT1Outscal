using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


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

    [SerializeField] private Image shieldPowerupImage;
    [SerializeField] private Image scoreBoostPowerupImage;
    [SerializeField] private Image speedupPowerupImage;

    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI highscoreText;

    [SerializeField] private TextMeshProUGUI finalScoreText;
    [SerializeField] private TextMeshProUGUI finalHighScoreText;

    private int score;
    private int highScore;


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


    public void PlayButtonSound()
    {
        SoundManager.Instance.Play(Sounds.BUTTON_CLICK);
    }


    public void ShieldPowerupActivated()
    {
        shieldPowerupImage.enabled = true;
    }


    public void ShieldPowerupDeactivated()
    {
        shieldPowerupImage.enabled = false;
    }


    public void ScoreBoostPowerupActivated()
    {
        scoreBoostPowerupImage.enabled = true;
    }


    public void ScoreBoostPowerupDeactivated()
    {
        scoreBoostPowerupImage.enabled = false;
    }


    public void SpeedupPowerupActivated()
    {
        speedupPowerupImage.enabled = true;
    }


    public void SpeedupPowerupDeactivated()
    {
        speedupPowerupImage.enabled = false;
    }


    public void DeactivateAllPowerups()
    {
        shieldPowerupImage.enabled = false;
        scoreBoostPowerupImage.enabled = false;
        speedupPowerupImage.enabled = false;
    }


    public void UpdateScore()
    {
        score = GameManager.Instance.Score;
        scoreText.text = score.ToString();

        highScore = GameManager.Instance.HighScore;
        highscoreText.text = highScore.ToString();
    }


    public void UpdateFinalScores()
    {
        finalScoreText.text = "Score: " + score.ToString();
        finalHighScoreText.text = "Highscore: " + highScore.ToString();
    }
}


