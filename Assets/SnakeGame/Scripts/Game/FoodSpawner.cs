
using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameConfig gameConfig;
    [SerializeField] private GridManager gridManager;
    [SerializeField] private SnakeController snakeController;
    [SerializeField] private Transform foodPrefab;

    [Header("Food Types")]
    [SerializeField] private FoodDefinition[] foodTypes;

    private Transform foodObject;
    private SpriteRenderer foodRenderer;
    private float lifetimeTimer;

    public Vector2Int FoodPosition { get; private set; }
    public bool HasFood { get; private set; }

    public FoodDefinition CurrentFood { get; private set; }

    private void Start()
    {
        if (gameConfig == null || gridManager == null ||
            snakeController == null || foodPrefab == null ||
            foodTypes == null || foodTypes.Length == 0)
        {
            Debug.LogError("FoodSpawner references are missing.");
            enabled = false;
            return;
        }

        foodObject = Instantiate(foodPrefab, transform);
        foodRenderer = foodObject.GetComponent<SpriteRenderer>();

        if (foodRenderer == null)
        {
            Debug.LogError("Food prefab needs a SpriteRenderer.");
            enabled = false;
            return;
        }

        SpawnFood();
    }

    private void Update()
    {
        
            if (snakeController == null ||
                snakeController.GameManagerState != GameState.Playing)
                return;

            // Keep your existing food lifetime code below.
        
        if (!HasFood || CurrentFood == null)
            return;

        if (CurrentFood.lifetime <= 0f)
            return;

        lifetimeTimer += Time.deltaTime;

        if (lifetimeTimer >= CurrentFood.lifetime)
            SpawnFood();
    }

    public bool IsFoodAt(Vector2Int position)
    {
        return HasFood && FoodPosition == position;
    }

    public void SpawnFood()
    {
        SnakeModel model = snakeController.Model;

        int emptyCellCount = 0;

        for (int x = 0; x < gameConfig.GridWidth; x++)
        {
            for (int y = 0; y < gameConfig.GridHeight; y++)
            {
                Vector2Int cell = new Vector2Int(x, y);

                if (!model.IsOccupied(cell))
                    emptyCellCount++;
            }
        }

        if (emptyCellCount == 0)
        {
            HasFood = false;
            CurrentFood = null;
            foodObject.gameObject.SetActive(false);
            return;
        }

        FoodDefinition selectedFood = ChooseFood();

        if (selectedFood == null)
        {
            Debug.LogError("No valid food definition found.");
            return;
        }

        int selectedCell = Random.Range(0, emptyCellCount);

        for (int x = 0; x < gameConfig.GridWidth; x++)
        {
            for (int y = 0; y < gameConfig.GridHeight; y++)
            {
                Vector2Int cell = new Vector2Int(x, y);

                if (model.IsOccupied(cell))
                    continue;

                if (selectedCell == 0)
                {
                    FoodPosition = cell;
                    CurrentFood = selectedFood;
                    HasFood = true;
                    lifetimeTimer = 0f;

                    foodObject.position =
                        gridManager.GetCellCenter(cell);

                    foodRenderer.sprite = selectedFood.foodSprite;
                    foodObject.localScale = selectedFood.foodScale;
                    foodRenderer.color = selectedFood.foodColor;
                    foodObject.gameObject.SetActive(true);
                    return;
                }

                selectedCell--;
            }
        }
    }

    private FoodDefinition ChooseFood()
    {
        float totalWeight = 0f;

        for (int i = 0; i < foodTypes.Length; i++)
        {
            if (foodTypes[i] != null &&
                foodTypes[i].spawnWeight > 0f)
            {
                totalWeight += foodTypes[i].spawnWeight;
            }
        }

        if (totalWeight <= 0f)
            return null;

        float randomValue = Random.Range(0f, totalWeight);

        for (int i = 0; i < foodTypes.Length; i++)
        {
            FoodDefinition food = foodTypes[i];

            if (food == null || food.spawnWeight <= 0f)
                continue;

            randomValue -= food.spawnWeight;

            if (randomValue < 0f)
                return food;
        }

        return null;
    }
}
