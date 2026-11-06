
using UnityEngine;

public class SnakeRenderer : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SnakeController snakeController;
    [SerializeField] private GridManager gridManager;
    [SerializeField] private GameConfig gameConfig;
    [SerializeField] private Transform segmentPrefab;

    private Transform[] segmentPool;

    private void Start()
    {
        if (snakeController == null ||
            gridManager == null ||
            gameConfig == null ||
            segmentPrefab == null)
        {
            Debug.LogError("SnakeRenderer references are missing.");
            enabled = false;
            return;
        }

        CreateSegmentPool();
    }

    private void CreateSegmentPool()
    {
        // Prepare enough segments for the entire grid.
        int poolSize = gameConfig.GridWidth *
                       gameConfig.GridHeight;

        segmentPool = new Transform[poolSize];

        for (int i = 0; i < poolSize; i++)
        {
            Transform segment = Instantiate(
                segmentPrefab,
                transform
            );

            segment.gameObject.SetActive(false);

            segmentPool[i] = segment;
        }
    }

    private void LateUpdate()
    {
        if (snakeController.Model == null)
            return;

        SnakeModel model = snakeController.Model;

        float progress = snakeController.TickProgress;

        for (int i = 0; i < segmentPool.Length; i++)
        {
            Transform segment = segmentPool[i];

            // Show only the segments currently used by the snake.
            bool shouldBeVisible = i < model.Length;

            if (segment.gameObject.activeSelf != shouldBeVisible)
            {
                segment.gameObject.SetActive(shouldBeVisible);
            }

            if (!shouldBeVisible)
                continue;

            Vector2Int previousPosition =
                snakeController.GetPreviousSegment(i);

            Vector2Int currentPosition =
                model.GetSegment(i);

            Vector3 startPosition =
                gridManager.GetCellCenter(previousPosition);

            Vector3 endPosition =
                gridManager.GetCellCenter(currentPosition);

            // Smoothly move between the two grid cells.
            segment.position = Vector3.Lerp(
                startPosition,
                endPosition,
                progress
            );
        }
    }
}
