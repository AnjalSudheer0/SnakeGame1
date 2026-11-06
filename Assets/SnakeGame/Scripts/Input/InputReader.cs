
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour
{
    [Header("Input Action")]
    [SerializeField] private InputActionReference moveAction;

    // Stores direction inputs until the next movement tick.
    private readonly Queue<Vector2Int> directionQueue =
        new Queue<Vector2Int>(8);

    private Vector2Int lastQueuedDirection = Vector2Int.right;

    private void OnEnable()
    {
        if (moveAction == null)
            return;

        moveAction.action.performed += OnMovePerformed;
        moveAction.action.Enable();
    }

    private void OnDisable()
    {
        if (moveAction == null)
            return;

        moveAction.action.performed -= OnMovePerformed;
        moveAction.action.Disable();
    }

    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();

        // Ignore small stick movements.
        if (input.sqrMagnitude < 0.25f)
            return;

        Vector2Int newDirection;

        // Choose the strongest axis.
        if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
        {
            newDirection = input.x > 0
                ? Vector2Int.right
                : Vector2Int.left;
        }
        else
        {
            newDirection = input.y > 0
                ? Vector2Int.up
                : Vector2Int.down;
        }

        QueueDirection(newDirection);
    }

    private void QueueDirection(Vector2Int newDirection)
    {
        // Ignore duplicate directions.
        if (newDirection == lastQueuedDirection)
            return;

        // Never allow an immediate reverse.
        if (newDirection == -lastQueuedDirection)
            return;

        directionQueue.Enqueue(newDirection);
        lastQueuedDirection = newDirection;
    }

    public bool TryGetNextDirection(out Vector2Int direction)
    {
        if (directionQueue.Count > 0)
        {
            direction = directionQueue.Dequeue();
            return true;
        }

        direction = Vector2Int.zero;
        return false;
    }

    public void ResetDirectionBuffer(Vector2Int currentDirection)
    {
        directionQueue.Clear();
        lastQueuedDirection = currentDirection;
    }
}
