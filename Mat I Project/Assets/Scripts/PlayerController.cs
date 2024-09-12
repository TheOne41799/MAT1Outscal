using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    private Vector2 currentDirection;
    private float moveInterval;
    private float moveTimer;
    private Vector2 gridSize = new Vector2(1, 1);

    [SerializeField] private GameObject snakeBodyPrefab;
    [SerializeField] private GameObject snakeTailPrefab;

    [SerializeField] private int initialBodyCount = 6;

    private List<GameObject> segments = new List<GameObject>();
    private List<Vector2> previousPositions = new List<Vector2>();

    [SerializeField] private int increaseLengthBy = 3;
    [SerializeField] private int decreaseLengthBy = 1;

    private FoodSpawningController foodSpawningController;


    public void InitializeSnake(float speed, Vector2 spawnPosition)
    {
        moveInterval = speed;
        transform.position = spawnPosition;
        currentDirection = Vector2.right;
        ResetSegments();
        foodSpawningController = FindObjectOfType<FoodSpawningController>();
    }


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

        if (moveTimer >= moveInterval)
        {
            StorePreviousPositions();
            MoveSnake();
            MoveSnakeBodyAndTailSegments();

            InputManager.Instance.CanChangeDirection = true;

            moveTimer = 0;
        }
    }


    private void MoveSnake()
    {
        Vector2 nextPosition = (Vector2)transform.position + currentDirection * gridSize;
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


    private void ResetSegments()
    {
        foreach (var segment in segments)
        {
            Destroy(segment);
        }
        segments.Clear();
        previousPositions.Clear();
        segments.Add(gameObject);

        Vector2 currentPos = transform.position;
        previousPositions.Add(currentPos);

        for (int i = 0; i < initialBodyCount; i++)
        {
            currentPos -= (Vector2)transform.right * gridSize.x;
            GameObject newSegment = Instantiate(snakeBodyPrefab);
            newSegment.transform.position = currentPos;
            segments.Add(newSegment);
            previousPositions.Add(currentPos);
        }

        AddSnakeTailSegment();
    }


    private void AddSnakeTailSegment()
    {
        GameObject newTail = Instantiate(snakeTailPrefab);
        newTail.transform.position = segments[segments.Count - 1].transform.position;
        segments.Add(newTail);
        previousPositions.Add(newTail.transform.position);
    }


    private void StorePreviousPositions()
    {
        for (int i = 0; i < segments.Count; i++)
        {
            previousPositions[i] = segments[i].transform.position;
        }
    }


    private void MoveSnakeBodyAndTailSegments()
    {
        for (int i = 1; i < segments.Count; i++)
        {
            segments[i].transform.position = previousPositions[i - 1];
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("MassGainer"))
        {
            ChangeSnakeLength(increaseLengthBy);
            foodSpawningController.OnFoodCollected();
        }
        else if (collision.CompareTag("MassBurner"))
        {
            ChangeSnakeLength(-decreaseLengthBy);
            foodSpawningController.OnFoodCollected();
        }
    }


    private void ChangeSnakeLength(int lengthChange)
    {
        if (lengthChange > 0)
        {
            for(int i = 0; i < lengthChange; i++)
            {
                AddBodySegment();
            }
        }
        else
        {
            for(int i = 0; i < Mathf.Abs(lengthChange); i++)
            {
                RemoveBodySegment();
            }
        }
    }


    private void AddBodySegment()
    {
        GameObject newSegment = Instantiate(snakeBodyPrefab);
        newSegment.transform.position = segments[segments.Count - 1].transform.position;
        segments.Insert(segments.Count - 1, newSegment);
        previousPositions.Add(newSegment.transform.position);
    }


    private void RemoveBodySegment()
    {
        if (segments.Count > 2)
        {
            GameObject segmentToRemove = segments[segments.Count - 2];
            segments.RemoveAt(segments.Count - 2);
            Destroy(segmentToRemove);
        }
    }
}
















