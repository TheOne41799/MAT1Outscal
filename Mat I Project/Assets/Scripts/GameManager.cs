using System.Collections;
using System.Collections.Generic;
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

    [SerializeField] private float moveInterval = 0.1f;

    private List<GameObject> activeSnakes = new List<GameObject>();


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
        StartGame();
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


    public void ChangeGameMode(GameMode gameMode)
    {
        currentGameMode = gameMode;
    }


    public void PlayerOneWinsInTwoPlayerMode()
    {
        Debug.Log("Player One Wins");
    }


    public void PlayerTwoWinsInTwoPlayerMode()
    {
        Debug.Log("Player Two Wins");
    }
}


public enum GameMode
{
    ONE_PLAYER,
    TWO_PLAYER
}
