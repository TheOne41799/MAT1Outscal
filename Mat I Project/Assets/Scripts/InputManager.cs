using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private static InputManager instance;
    public static InputManager Instance {  get { return instance; } }

    private Direction currentMovementDirection = Direction.RIGHT;
    public Direction CurrentMovementDirection {  get { return currentMovementDirection; } }

    private bool canChangeDirection = true;
    public bool CanChangeDirection { set { canChangeDirection = value; } get {return canChangeDirection;} }


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
        if (!canChangeDirection) return;

        if (Input.GetKeyDown(KeyCode.UpArrow) && currentMovementDirection != Direction.DOWN)
        {
            currentMovementDirection = Direction.UP;
            canChangeDirection = false;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) && currentMovementDirection != Direction.UP)
        {
            currentMovementDirection = Direction.DOWN;
            canChangeDirection = false;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) && currentMovementDirection != Direction.LEFT)
        {
            currentMovementDirection = Direction.RIGHT;
            canChangeDirection = false;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) && currentMovementDirection != Direction.RIGHT)
        {
            currentMovementDirection = Direction.LEFT;
            canChangeDirection = false;
        }
    }
}


public enum Direction
{
    UP,
    DOWN,
    LEFT,
    RIGHT
}
