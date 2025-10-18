using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-100)]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [HideInInspector] public bool IsPaused = false;
    public float matchDuration = 600f; // 10 minutos
    private float timer;

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
        if (scene.name == "Game")
        {
            ResetGameState();
        }
    }

    void Start()
    {
        timer = matchDuration;
    }

    void Update()
    {
        if (IsPaused) return;
        timer -= Time.deltaTime;
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateTimer(timer);
        }
        if (timer <= 0f)
        {
            EndRun((int)matchDuration);
        }
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
        PlayerPrefs.SetInt("currency", current + secondsSurvived);
        PlayerPrefs.Save();
        PersistentUpgrades.Instance.Load();
        PauseGame();
        if (UIManager.Instance != null)
            UIManager.Instance.ShowEndRun(secondsSurvived);
    }

    public void RestartLevel()
    {
        Debug.Log("Reiniciando nivel...");
        Time.timeScale = 1f;
        SceneManager.LoadScene("Game");
    }

    public void BackToMenu()
    {
        Debug.Log("Volviendo al menú...");
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    private void ResetGameState()
    {
        Debug.Log("GameManager: reiniciando estado de partida...");
        IsPaused = false;
        Time.timeScale = 1f;
        timer = matchDuration;
    }
}
