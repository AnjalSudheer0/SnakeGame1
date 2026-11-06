
using System;
using UnityEngine;

public class SnakeModel
{
    // Stores every snake segment without creating new arrays during movement.
    private readonly Vector2Int[] segments;

    private readonly int gridWidth;
    private readonly int gridHeight;

    private int length;
    private Vector2Int direction;

    public int Length => length;

    public Vector2Int Direction => direction;

    public Vector2Int Head => segments[0];

    public SnakeModel(int width, int height, int startingLength)
    {
        if (width <= 0 || height <= 0)
            throw new ArgumentOutOfRangeException("Grid dimensions must be positive.");

        if (startingLength < 1 || startingLength > width)
            throw new ArgumentOutOfRangeException("Invalid starting length.");

        gridWidth = width;
        gridHeight = height;

        // Allocate enough space for the entire grid.
        segments = new Vector2Int[width * height];

        Reset(startingLength);
    }

    public void Reset(int startingLength)
    {
        if (startingLength < 1 || startingLength > gridWidth ||
            startingLength > segments.Length)
            throw new ArgumentOutOfRangeException(nameof(startingLength));

        length = startingLength;

        direction = Vector2Int.right;

        int startX = Mathf.Max(gridWidth / 2, startingLength - 1);
        int startY = gridHeight / 2;

        // Place the body behind the head.
        for (int i = 0; i < length; i++)
        {
            segments[i] = new Vector2Int(startX - i, startY);
        }
    }

    public bool TrySetDirection(Vector2Int newDirection)
    {
        // Only allow movement in four directions.
        bool isValidDirection =
            Mathf.Abs(newDirection.x) + Mathf.Abs(newDirection.y) == 1;

        if (!isValidDirection)
            return false;

        // The snake cannot immediately reverse direction.
        if (newDirection == -direction)
            return false;

        direction = newDirection;

        return true;
    }


    public void MoveOneCell(int lengthChange, int minimumLength)
    {
        int newLength = GetLengthAfterChange(
            lengthChange,
            minimumLength
        );

        length = newLength;

        for (int i = length - 1; i > 0; i--)
        {
            segments[i] = segments[i - 1];
        }

        segments[0] += direction;
    }


    public Vector2Int GetSegment(int index)
    {
        if (index < 0 || index >= length)
            throw new ArgumentOutOfRangeException(nameof(index));

        return segments[index];
    }

    public bool IsOccupied(Vector2Int position)
    {
        for (int i = 0; i < length; i++)
        {
            if (segments[i] == position)
                return true;
        }

        return false;
    }

    public int GetLengthAfterChange(int lengthChange, int minimumLength)
    {
        return Mathf.Clamp(
            length + lengthChange,
            minimumLength,
            segments.Length
        );
    }
}
