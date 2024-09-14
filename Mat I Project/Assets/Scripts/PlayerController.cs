


using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public int playerID = 1;

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

    private int score;
    public int Score { set { score = value; } get { return score; } }

    private bool isShieldPowerupActive = false;
    public bool IsShieldPowerupActive { get { return isShieldPowerupActive; } }
    private bool isScoreBoostPowerupActive = false;
    private bool isSpeedupPowerupActive = false;

    private float originalMoveIntervalBeforeSpeedupPowerup;


    public void InitializeSnake(float speed, Vector2 spawnPosition)
    {
        moveInterval = speed;
        transform.position = spawnPosition;
        //currentDirection = Vector2.right;

        InputManager.Instance.ResetPlayerDirectionToDefault();
        
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
        if (playerID == 1)
        {
            switch (InputManager.Instance.PlayerOneCurrentMovementDirection)
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
        else if (playerID == 2)
        {
            switch (InputManager.Instance.PlayerTwoCurrentMovementDirection)
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
    }


    private void MoveSnakeWithTimer()
    {
        moveTimer += Time.deltaTime;

        if (moveTimer >= moveInterval)
        {
            StorePreviousPositions();
            MoveSnake();
            MoveSnakeBodyAndTailSegments();

            InputManager.Instance.CanChangeDirectionPlayerOne = true;

            if (GameManager.Instance.CurrentGameMode == GameMode.TWO_PLAYER)
            {
                InputManager.Instance.CanChangeDirectionPlayerTwo = true;
            }

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

            if (playerID == 1)
            {
                newSegment.tag = "PlayerOneBodyOrTail";
            }
            else if (playerID == 2)
            {
                newSegment.tag = "PlayerTwoBodyOrTail";
            }

            segments.Add(newSegment);
            previousPositions.Add(currentPos);
        }

        AddSnakeTailSegment();
    }


    private void AddSnakeTailSegment()
    {
        GameObject newTail = Instantiate(snakeTailPrefab);
        newTail.transform.position = segments[segments.Count - 1].transform.position;

        if (playerID == 1)
        {
            newTail.tag = "PlayerOneBodyOrTail";
        }
        else if (playerID == 2)
        {
            newTail.tag = "PlayerTwoBodyOrTail";
        }

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

            UpdateScore(3);

            Destroy(collision.gameObject);
        }
        else if (collision.CompareTag("MassBurner"))
        {
            ChangeSnakeLength(-decreaseLengthBy);
            foodSpawningController.OnFoodCollected();

            UpdateScore(-1);

            Destroy(collision.gameObject);
        }
        else if (collision.CompareTag("ShieldPowerup"))
        {
            ActivateShield();
            Destroy(collision.gameObject);
        }
        else if (collision.CompareTag("ScoreBoostPowerup"))
        {
            ActivateScoreBoost();
            Destroy(collision.gameObject);
        }
        else if (collision.CompareTag("SpeedupPowerup"))
        {
            ActivateSpeedup();
            Destroy(collision.gameObject);
        }
    }


    private void ChangeSnakeLength(int lengthChange)
    {
        if (lengthChange > 0)
        {
            for (int i = 0; i < lengthChange; i++)
            {
                AddBodySegment();
            }
        }
        else
        {
            for (int i = 0; i < Mathf.Abs(lengthChange); i++)
            {
                RemoveBodySegment();
            }
        }
    }


    private void AddBodySegment()
    {
        GameObject newSegment = Instantiate(snakeBodyPrefab);
        newSegment.transform.position = segments[segments.Count - 1].transform.position;

        if (playerID == 1)
        {
            newSegment.tag = "PlayerOneBodyOrTail";
        }
        else if (playerID == 2)
        {
            newSegment.tag = "PlayerTwoBodyOrTail";
        }

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


    public void RemoveAllSegments()
    {
        foreach (var segment in segments)
        {
            Destroy(segment);
        }
        segments.Clear();
        previousPositions.Clear();
    }


    private void UpdateScore(int scoreAmount)
    {
        if (isScoreBoostPowerupActive) scoreAmount *= 2;

        score += scoreAmount;

        if (score < 0) score = 0;
    }


    private void ActivateShield()
    {
        Debug.Log("Shield Activated");

        isShieldPowerupActive = true;

        StartCoroutine(DeactivateShield(10f));
    }


    private IEnumerator DeactivateShield(float time)
    {
        yield return new WaitForSeconds(time);

        isShieldPowerupActive = false;

        Debug.Log("Shield Deactivated");
    }


    private void ActivateScoreBoost()
    {
        Debug.Log("Score Boost Activated");

        isScoreBoostPowerupActive = true;

        StartCoroutine(DeactivateScoreBoost(10f));
    }


    private IEnumerator DeactivateScoreBoost(float time)
    {
        yield return new WaitForSeconds(time);

        isScoreBoostPowerupActive = false;

        Debug.Log("Score Boost Deactivated");
    }


    private void ActivateSpeedup()
    {
        Debug.Log("Speedup Activated");

        isSpeedupPowerupActive = true;

        originalMoveIntervalBeforeSpeedupPowerup = moveInterval;

        moveInterval = 0.05f;

        StartCoroutine(DeactivateSpeedup(10f));
    }


    private IEnumerator DeactivateSpeedup(float time)
    {
        yield return new WaitForSeconds(time);

        isSpeedupPowerupActive = false;

        moveInterval = originalMoveIntervalBeforeSpeedupPowerup;

        Debug.Log("Speedup Deactivated");
    }
}































/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public int playerID = 1;

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

    private int score;
    public int Score { set { score = value; } get { return score; } }

    private bool isShieldPowerupActive = false;
    public bool IsShieldPowerupActive { get { return isShieldPowerupActive; } }
    private bool isScoreBoostPowerupActive = false;
    private bool isSpeedupPowerupActive = false;

    private float originalMoveIntervalBeforeSpeedupPowerup;


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
        if (playerID == 1)
        {
            switch (InputManager.Instance.PlayerOneCurrentMovementDirection)
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
        else if (playerID == 2)
        {
            switch (InputManager.Instance.PlayerTwoCurrentMovementDirection)
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
    }


    private void MoveSnakeWithTimer()
    {
        moveTimer += Time.deltaTime;

        if (moveTimer >= moveInterval)
        {
            StorePreviousPositions();
            MoveSnake();
            MoveSnakeBodyAndTailSegments();

            InputManager.Instance.CanChangeDirectionPlayerOne = true;

            if (GameManager.Instance.CurrentGameMode == GameMode.TWO_PLAYER)
            {
                InputManager.Instance.CanChangeDirectionPlayerTwo = true;
            }

            //CheckSelfCollision();

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

            if (playerID == 1)
            {
                newSegment.tag = "PlayerOneBodyOrTail";
            }
            else if (playerID == 2)
            {
                newSegment.tag = "PlayerTwoBodyOrTail";
            }

            segments.Add(newSegment);
            previousPositions.Add(currentPos);
        }

        AddSnakeTailSegment();
    }


    private void AddSnakeTailSegment()
    {
        GameObject newTail = Instantiate(snakeTailPrefab);
        newTail.transform.position = segments[segments.Count - 1].transform.position;

        if (playerID == 1)
        {
            newTail.tag = "PlayerOneBodyOrTail";
        }
        else if (playerID == 2)
        {
            newTail.tag = "PlayerTwoBodyOrTail";
        }

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

            UpdateScore(3);

            Destroy(collision.gameObject);
        }
        else if (collision.CompareTag("MassBurner"))
        {
            ChangeSnakeLength(-decreaseLengthBy);
            foodSpawningController.OnFoodCollected();

            UpdateScore(-1);

            Destroy(collision.gameObject);
        }
        else if(collision.CompareTag("ShieldPowerup"))
        {
            ActivateShield();
            Destroy(collision.gameObject);
        }
        else if(collision.CompareTag("ScoreBoostPowerup"))
        {
            ActivateScoreBoost();
            Destroy(collision.gameObject);
        }
        else if(collision.CompareTag("SpeedupPowerup"))
        {
            ActivateSpeedup();
            Destroy(collision.gameObject);
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

        if (playerID == 1)
        {
            newSegment.tag = "PlayerOneBodyOrTail";
        }
        else if (playerID == 2)
        {
            newSegment.tag = "PlayerTwoBodyOrTail";
        }

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


    public void RemoveAllSegments()
    {
        foreach (var segment in segments)
        {
            Destroy(segment);
        }
        segments.Clear();
        previousPositions.Clear();
    }


    *//*private void CheckSelfCollision()
    {
        if (isShieldPowerupActive) return;

        for (int i = 1; i < segments.Count; i++)
        {
            if(Vector2.Distance(transform.position, segments[i].transform.position) < 0.1f)
            {
                GameOver();
                break;
            }
        }
    }*/


/*private void GameOver()
{
    //Time.timeScale = 0;
}*//*


private void UpdateScore(int scoreAmount)
{
    if (isScoreBoostPowerupActive) scoreAmount *= 2;

    score += scoreAmount;

    if(score < 0) score = 0;
}


private void ActivateShield()
{
    Debug.Log("Shield Activated");

    isShieldPowerupActive = true;

    StartCoroutine(DeactivateShield(10f));
}


private IEnumerator DeactivateShield(float time)
{
    yield return new WaitForSeconds(time);

    isShieldPowerupActive = false;

    Debug.Log("Shield Deactivated");
}


private void ActivateScoreBoost()
{
    Debug.Log("Score Boost Activated");

    isScoreBoostPowerupActive = true;

    StartCoroutine(DeactivateScoreBoost(10f));
}


private IEnumerator DeactivateScoreBoost(float time)
{
    yield return new WaitForSeconds(time);

    isScoreBoostPowerupActive = false;

    Debug.Log("Score Boost Deactivated");
}


private void ActivateSpeedup()
{
    Debug.Log("Speedup Activated");

    isSpeedupPowerupActive = true;

    originalMoveIntervalBeforeSpeedupPowerup = moveInterval;

    moveInterval = 0.05f;

    StartCoroutine(DeactivateSpeedup(10f));
}


private IEnumerator DeactivateSpeedup(float time)
{
    yield return new WaitForSeconds(time);

    isSpeedupPowerupActive = false;

    moveInterval = originalMoveIntervalBeforeSpeedupPowerup;

    Debug.Log("Speedup Deactivated");
}
}
















*/