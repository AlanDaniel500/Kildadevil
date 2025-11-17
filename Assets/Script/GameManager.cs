using System;
using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-100)]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [HideInInspector] public bool IsPaused = false;
    public float matchDuration = 120f; // segundos
    private float timer;
    private bool timerFinished = false;
    private string[] levels = new string[]
        {
            "Game",
            "Game2",
            "Game3",
            "BossTest"
        };
    public string lastPlayed;
    public float levelMult;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        bool game = Array.IndexOf(levels, scene.name) >= 0;
        if (game == true)
        {
            lastPlayed = scene.name;
            ResetGameState();
        }
        switch (scene.name)
        {
            case ("BossTest"):
                levelMult = 1f;
                timer = 10f;
                break;
            case ("Game"):
                levelMult = 1f;
                break;
            case ("Game2"):
                levelMult = 2f;
                break;
            case ("Game3"):
                levelMult = 3f;
                break;
        }
    }

    void Start()
    {
        timer = matchDuration;
    }

    void Update()
    {
        if (IsPaused) return;
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateTimer(timer);
        }
        if (timer <= 0f && !timerFinished)
        {
            timerFinished = true;
            DestroyEnemies();
            SpawnBoss();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    private void DestroyEnemies()
    {
        Destroy(FindFirstObjectByType<EnemySpawner>());
        var enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        foreach (var ene in enemies)
        {
            if (ene != null) Destroy(ene.gameObject);
        }
    }

    private void SpawnBoss()
    {
        BossSpawner.Instance.SpawnBoss();
    }

    public void PauseGame()
    {
        IsPaused = true;
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        IsPaused = false;
        Time.timeScale = 1f;
    }

    public void EndRun(int secondsSurvived)
    {
        int current = PlayerPrefs.GetInt("currency", 0);
        PlayerPrefs.SetInt("currency", current + secondsSurvived * 5);
        PlayerPrefs.Save();
        PersistentUpgrades.Instance.Load();
        PauseGame();
        if (UIManager.Instance != null)
            UIManager.Instance.ShowEndRun(secondsSurvived * 5);
    }

    public void RestartLevel()
    {
        Debug.Log("Reiniciando nivel...");
        timerFinished = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene(lastPlayed);
    }

    public void BackToMenu()
    {
        Debug.Log("Volviendo al menú...");
        timerFinished = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    private void ResetGameState()
    {
        Debug.Log("GameManager: reiniciando estado de partida...");
        IsPaused = false;
        timerFinished = false;
        Time.timeScale = 1f;
        timer = matchDuration;
    }
}
