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


    private void Update()
    {
        
    }


    private void StartGame()
    {
        ClearExistingSnakes();


        switch (currentGameMode)
        {
            case GameMode.ONE_PLAYER:
                SpawnSnake(spawnPointPlayer1.position, moveInterval); 
                break;
            case GameMode.TWO_PLAYER:
                SpawnSnake(spawnPointPlayer1.position, moveInterval);
                SpawnSnake(spawnPointPlayer2.position, moveInterval);
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


    private void SpawnSnake(Vector2 spawnPosition, float speed)
    {
        GameObject newSnake = Instantiate(snakePrefab, spawnPosition, Quaternion.identity);
        newSnake.GetComponent<PlayerController>().InitializeSnake(speed, spawnPosition);
        activeSnakes.Add(newSnake);
    }


    public void ChangeGameMode(GameMode gameMode)
    {
        currentGameMode = gameMode;
    }
}


public enum GameMode
{
    ONE_PLAYER,
    TWO_PLAYER
}
