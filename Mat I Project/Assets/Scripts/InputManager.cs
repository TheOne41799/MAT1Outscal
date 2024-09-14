using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InputManager : MonoBehaviour
{
    private static InputManager instance;
    public static InputManager Instance { get { return instance; } }

    private Direction playerOneCurrentMovementDirection = Direction.RIGHT;
    private Direction playerTwoCurrentMovementDirection = Direction.RIGHT;

    public Direction PlayerOneCurrentMovementDirection { get { return playerOneCurrentMovementDirection; } }
    public Direction PlayerTwoCurrentMovementDirection { get { return playerTwoCurrentMovementDirection; } }

    private bool canChangeDirectionPlayerOne = true;
    private bool canChangeDirectionPlayerTwo = true;

    public bool CanChangeDirectionPlayerOne
    {
        set { canChangeDirectionPlayerOne = value; }
        get { return canChangeDirectionPlayerOne; }
    }
    public bool CanChangeDirectionPlayerTwo
    {
        set { canChangeDirectionPlayerTwo = value; }
        get { return canChangeDirectionPlayerTwo; }
    }


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


    private void Update()
    {
        HandleUserInput();
    }


    private void HandleUserInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameManager.Instance.CurrentGameState == GameState.IN_GAME_UI_ONE_PLAYER ||
               GameManager.Instance.CurrentGameState == GameState.IN_GAME_UI_TWO_PLAYER)
            {
                SoundManager.Instance.Play(Sounds.PAUSE);
                GameManager.Instance.SetGameState(GameState.PAUSE_MENU);
            }
        }


        if (canChangeDirectionPlayerOne)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) && playerOneCurrentMovementDirection != Direction.DOWN)
            {
                playerOneCurrentMovementDirection = Direction.UP;
                canChangeDirectionPlayerOne = false;
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow) && playerOneCurrentMovementDirection != Direction.UP)
            {
                playerOneCurrentMovementDirection = Direction.DOWN;
                canChangeDirectionPlayerOne = false;
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) && playerOneCurrentMovementDirection != Direction.LEFT)
            {
                playerOneCurrentMovementDirection = Direction.RIGHT;
                canChangeDirectionPlayerOne = false;
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow) && playerOneCurrentMovementDirection != Direction.RIGHT)
            {
                playerOneCurrentMovementDirection = Direction.LEFT;
                canChangeDirectionPlayerOne = false;
            }
        }

        if (canChangeDirectionPlayerTwo)
        {
            if (Input.GetKeyDown(KeyCode.W) && playerTwoCurrentMovementDirection != Direction.DOWN)
            {
                playerTwoCurrentMovementDirection = Direction.UP;
                canChangeDirectionPlayerTwo = false;
            }
            else if (Input.GetKeyDown(KeyCode.S) && playerTwoCurrentMovementDirection != Direction.UP)
            {
                playerTwoCurrentMovementDirection = Direction.DOWN;
                canChangeDirectionPlayerTwo = false;
            }
            else if (Input.GetKeyDown(KeyCode.D) && playerTwoCurrentMovementDirection != Direction.LEFT)
            {
                playerTwoCurrentMovementDirection = Direction.RIGHT;
                canChangeDirectionPlayerTwo = false;
            }
            else if (Input.GetKeyDown(KeyCode.A) && playerTwoCurrentMovementDirection != Direction.RIGHT)
            {
                playerTwoCurrentMovementDirection = Direction.LEFT;
                canChangeDirectionPlayerTwo = false;
            }
        }
    }


    public void ResetPlayerDirectionToDefault()
    {
        playerOneCurrentMovementDirection = Direction.RIGHT;
        playerTwoCurrentMovementDirection = Direction.RIGHT;
    }
}


public enum Direction
{
    UP,
    DOWN,
    LEFT,
    RIGHT
}




