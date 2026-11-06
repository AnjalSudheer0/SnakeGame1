
using UnityEngine;

public enum FoodEffect
{
    Grow,
    Shrink,
    ScoreOnly
}

[CreateAssetMenu(
    fileName = "NewFood",
    menuName = "Snake Game/Food Definition"
)]
public class FoodDefinition : ScriptableObject
{
    [Header("Food Information")]
    public string foodName;
    public Sprite foodSprite;

    [Header("Spawn Settings")]
    [Min(0f)] public float spawnWeight = 1f;

    [Header("Food Effect")]
    public FoodEffect effect;
    public int lengthChange;
    public int minimumSnakeLength = 3;
    public int scoreValue = 10;

    [Header("Lifetime")]
    [Min(0f)] public float lifetime = 0f;

    [Header("Visual Settings")]
    public Vector3 foodScale = Vector3.one;
    public Color foodColor = Color.white;
}
