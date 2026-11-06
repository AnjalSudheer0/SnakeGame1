
using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Snake Game/Game Config")]
public class GameConfig : ScriptableObject
{
    [Header("Grid Settings")]
    [SerializeField] private int gridWidth = 32;
    [SerializeField] private int gridHeight = 18;
    [SerializeField] private float cellSize = 0.5f;

    [Header("Movement Settings")]
    [SerializeField] private float tickInterval = 0.15f;
    [SerializeField] private int startingLength = 3;

    [Header("Speed Progression")]
    [SerializeField] private int foodsPerSpeedIncrease = 5;

    [SerializeField]
    private AnimationCurve tickIntervalBySpeedLevel =
        new AnimationCurve(
            new Keyframe(0, 0.15f),
            new Keyframe(1, 0.13f),
            new Keyframe(2, 0.11f),
            new Keyframe(3, 0.09f),
            new Keyframe(4, 0.07f)
        );

    public int FoodsPerSpeedIncrease => foodsPerSpeedIncrease;

    public float GetTickInterval(int speedLevel)
    {
        return Mathf.Max(
            0.03f,
            tickIntervalBySpeedLevel.Evaluate(speedLevel)
        );
    }

    public int GridWidth => gridWidth;
    public int GridHeight => gridHeight;
    public float CellSize => cellSize;

    public float TickInterval => tickInterval;
    public int StartingLength => startingLength;
}
