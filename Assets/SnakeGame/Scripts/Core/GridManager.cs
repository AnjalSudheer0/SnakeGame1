
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [Header("Grid Configuration")]
    [SerializeField] private GameConfig gameConfig;

    // Checks whether a grid coordinate is inside the board.
    public bool IsInsideGrid(Vector2Int position)
    {
        return position.x >= 0 &&
               position.x < gameConfig.GridWidth &&
               position.y >= 0 &&
               position.y < gameConfig.GridHeight;
    }

    // Converts a grid coordinate into a Unity world position.
    public Vector3 GetCellCenter(Vector2Int position)
    {
        float cellSize = gameConfig.CellSize;

        float x = (position.x + 0.5f) * cellSize;
        float y = (position.y + 0.5f) * cellSize;

        float offsetX = gameConfig.GridWidth * cellSize / 2f;
        float offsetY = gameConfig.GridHeight * cellSize / 2f;

        return transform.position +
               new Vector3(x - offsetX, y - offsetY, 0f);
    }

    // Converts a Unity world position into grid coordinates.
    public Vector2Int WorldToCell(Vector3 worldPosition)
    {
        Vector3 localPosition = worldPosition - transform.position;

        float offsetX = gameConfig.GridWidth * gameConfig.CellSize / 2f;
        float offsetY = gameConfig.GridHeight * gameConfig.CellSize / 2f;

        int x = Mathf.FloorToInt(
            (localPosition.x + offsetX) / gameConfig.CellSize);

        int y = Mathf.FloorToInt(
            (localPosition.y + offsetY) / gameConfig.CellSize);

        return new Vector2Int(x, y);
    }

    // Draws the grid in the Unity Scene view.
    private void OnDrawGizmos()
    {
        if (gameConfig == null)
            return;

        Gizmos.color = Color.gray;

        float cellSize = gameConfig.CellSize;

        float width = gameConfig.GridWidth * cellSize;
        float height = gameConfig.GridHeight * cellSize;

        Vector3 center = transform.position;

        float left = center.x - width / 2f;
        float right = center.x + width / 2f;
        float bottom = center.y - height / 2f;
        float top = center.y + height / 2f;

        // Draw vertical lines.
        for (int x = 0; x <= gameConfig.GridWidth; x++)
        {
            float positionX = left + x * cellSize;

            Gizmos.DrawLine(
                new Vector3(positionX, bottom, 0f),
                new Vector3(positionX, top, 0f));
        }

        // Draw horizontal lines.
        for (int y = 0; y <= gameConfig.GridHeight; y++)
        {
            float positionY = bottom + y * cellSize;

            Gizmos.DrawLine(
                new Vector3(left, positionY, 0f),
                new Vector3(right, positionY, 0f));
        }
    }
}
