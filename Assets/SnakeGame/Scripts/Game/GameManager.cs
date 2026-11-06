
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SnakeController snakeController;
    [SerializeField] private FoodSpawner foodSpawner;

    [Header("Menu UI")]
    [SerializeField] private GameObject mainMenuPanel;
    //[SerializeField] private GameObject gameplayHUD;
    [SerializeField] private TMP_Text menuHighScoreText;

    [Header("GamePlay UI")]
    [SerializeField] private TMP_Text GamePlayScore;
    [SerializeField] private TMP_Text GamePlayHighScoreScore;

    [Header("GameOver UI")]
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text highScoreText;
    [SerializeField] private GameObject gameOverPanel;

    [Header("Pause UI")]
    [SerializeField] private GameObject pausePanel;

    private ISaveService saveService;
    private SaveData saveData;

    public GameState CurrentState { get; private set; }

    private int score;

    private void Awake()
    {
        saveService = new JsonSaveService();
        saveData = saveService.Load();

        CurrentState = GameState.Playing;
    }

    private void Start()
    {
        score = 0;
        CurrentState = GameState.MainMenu;

        mainMenuPanel.SetActive(true);
        //gameplayHUD.SetActive(false);
        gameOverPanel.SetActive(false);

        UpdateUI();
    }

    public void StartGame()
    {
        score = 0;
        CurrentState = GameState.Playing;

        mainMenuPanel.SetActive(false);
        //gameplayHUD.SetActive(true);
        gameOverPanel.SetActive(false);

        snakeController.ResetSnake();
        foodSpawner.SpawnFood();

        UpdateUI();
    }

    public void PauseGame()
    {
        if (CurrentState != GameState.Playing)
            return;

        CurrentState = GameState.Paused;
        pausePanel.SetActive(true);
    }

    public void ResumeGame()
    {
        if (CurrentState != GameState.Paused)
            return;

        pausePanel.SetActive(false);
        CurrentState = GameState.Playing;
    }

    public void ReturnHome()
    {
        CurrentState = GameState.MainMenu;

        pausePanel.SetActive(false);
        //gameplayHUD.SetActive(false);
        //howToPlayPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    public void AddScore(int amount)
    {
        if (CurrentState != GameState.Playing)
            return;

        score += amount;

        if (score > saveData.highScore)
        {
            saveData.highScore = score;
            saveService.Save(saveData);
        }

        UpdateUI();
    }

    public void GameOver()
    {
        if (CurrentState != GameState.Playing)
            return;

        CurrentState = GameState.GameOver;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        UpdateUI();
    }

    public void RestartGame()
    {
        score = 0;
        CurrentState = GameState.Playing;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        snakeController.ResetSnake();
        foodSpawner.SpawnFood();

        UpdateUI();
    }

    private void UpdateUI()
    {
        scoreText.text = "" + score;

        highScoreText.text = "" + saveData.highScore;

        menuHighScoreText.text =
            "" + saveData.highScore;

        GamePlayScore.text = "" + score;
        GamePlayHighScoreScore.text = "" + saveData.highScore;
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game Quit");
    }
}
