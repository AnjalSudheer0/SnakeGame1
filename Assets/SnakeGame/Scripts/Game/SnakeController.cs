
using UnityEngine;

public class SnakeController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameConfig gameConfig;
    [SerializeField] private InputReader inputReader;
    [SerializeField] private FoodSpawner foodSpawner;

    [SerializeField] private GridManager gridManager;
    [SerializeField] private GameManager gameManager;

    private SnakeModel snakeModel;

    // Stores the snake positions before its last movement.
    private Vector2Int[] previousSegments;
    private int previousLength;

    private float movementTimer;

    public SnakeModel Model => snakeModel;

    public GameState GameManagerState =>
    gameManager.CurrentState;

    public int PreviousLength => previousLength;

    private int foodsEaten;

    public float CurrentTickInterval =>
        gameConfig.GetTickInterval(
            foodsEaten / gameConfig.FoodsPerSpeedIncrease
        );

    public float TickProgress =>
     Mathf.Clamp01(movementTimer / CurrentTickInterval);

    private void Awake()
    {
        if (gameConfig == null || inputReader == null)
        {
            Debug.LogError("SnakeController references are missing.");
            enabled = false;
            return;
        }

        snakeModel = new SnakeModel(
            gameConfig.GridWidth,
            gameConfig.GridHeight,
            gameConfig.StartingLength
        );

        previousSegments = new Vector2Int[
            gameConfig.GridWidth * gameConfig.GridHeight
        ];

        SavePreviousPositions();
    }

    private void Start()
    {
        inputReader.ResetDirectionBuffer(snakeModel.Direction);
    }


    private void Update()
    {
        if (gameManager == null ||
            gameManager.CurrentState != GameState.Playing)
            return;

        movementTimer += Time.deltaTime;

        while (movementTimer >= CurrentTickInterval)
        {
            movementTimer -= CurrentTickInterval;
            MoveSnake();
        }
    }


    private void MoveSnake()
    {
        if (inputReader.TryGetNextDirection(out Vector2Int newDirection))
            snakeModel.TrySetDirection(newDirection);

        Vector2Int nextPosition =
            snakeModel.Head + snakeModel.Direction;

        FoodDefinition food = null;

        if (foodSpawner != null &&
            foodSpawner.IsFoodAt(nextPosition))
        {
            food = foodSpawner.CurrentFood;
        }

        int lengthChange = food != null ? food.lengthChange : 0;
        int minimumLength = food != null
            ? food.minimumSnakeLength
            : 3;

        int nextLength = snakeModel.GetLengthAfterChange(
            lengthChange,
            minimumLength
        );

        // Wall collision
        if (!gridManager.IsInsideGrid(nextPosition))
        {
            gameManager.GameOver();
            return;
        }

        // Self collision.
        // The old tail cell is excluded because it moves away.
        int collisionCount = nextLength - 1;

        for (int i = 0; i < collisionCount; i++)
        {
            if (snakeModel.GetSegment(i) == nextPosition)
            {
                gameManager.GameOver();
                return;
            }
        }

        SavePreviousPositions();

        snakeModel.MoveOneCell(lengthChange, minimumLength);

        if (food != null)
        {
            foodsEaten++;

            gameManager.AddScore(food.scoreValue);

            foodSpawner.SpawnFood();
        }
    }


    public void ResetSnake()
    {
        snakeModel.Reset(gameConfig.StartingLength);

        movementTimer = 0f;
        foodsEaten = 0;

        SavePreviousPositions();

        inputReader.ResetDirectionBuffer(snakeModel.Direction);
    }

    private void SavePreviousPositions()
    {
        previousLength = snakeModel.Length;

        for (int i = 0; i < previousLength; i++)
        {
            previousSegments[i] = snakeModel.GetSegment(i);
        }
    }

    public Vector2Int GetPreviousSegment(int index)
    {
        if (index < 0 || index >= previousSegments.Length)
            return Vector2Int.zero;

        if (index < previousLength)
            return previousSegments[index];

        // A newly grown segment starts at the old tail position.
        return previousSegments[previousLength - 1];
    }
}
