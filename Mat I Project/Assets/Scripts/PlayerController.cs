using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    private Vector2 currentDirection;
    private float moveInterval = 0.1f;
    private float moveTimer;
    private Vector2 gridSize = new Vector2(1, 1);


    private void Update()
    {
        HandleInput();

        MoveSnakeWithTimer();
    }


    private void HandleInput()
    {
        switch (InputManager.Instance.CurrentMovementDirection)
        {
            case Direction.UP:
                currentDirection = Vector2.up;
                break;
            case Direction.DOWN:
                currentDirection = Vector2.down;
                break;
            case Direction.LEFT:
                currentDirection = Vector2.left;
                break;
            case Direction.RIGHT:
                currentDirection = Vector2.right;
                break;
        }
    }


    private void MoveSnakeWithTimer()
    {
        moveTimer += Time.deltaTime;

        if (moveTimer > moveInterval)
        {
            MoveSnake();

            InputManager.Instance.CanChangeDirection = true;

            moveTimer = 0;
        }
    }


    private void MoveSnake()
    {
        Vector2 nextPosition = (Vector2) transform.position + currentDirection * gridSize;
        transform.position = WrapSnakeAroundScreen(nextPosition);
    }


    private Vector2 WrapSnakeAroundScreen(Vector2 position)
    {
        if (position.x > 19) position.x = -19;
        else if (position.x < -19) position.x = 19;
        if (position.y > 19) position.y = -19;
        else if (position.y < -19) position.y = 19;

        return position;
    }
}








