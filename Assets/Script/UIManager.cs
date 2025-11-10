using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public GameObject deathPanel;
    public GameObject levelUpPanel;
    public Button retryButton;
    public Button mainMenuButton;
    public Text timerText;
    public Text endRunText;
    public float time;

    void Awake()
    { 
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Escuchar cuando se cargue una nueva escena
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Cada vez que se carga una escena nueva, intentamos reconectar referencias
        StartCoroutine(FindUIReferences());
    }

    IEnumerator FindUIReferences()
    {
        // Esperar un frame para que los objetos de la escena estén inicializados
        yield return null;

        // Buscar incluso si los paneles están desactivados
        var allObjects = Resources.FindObjectsOfTypeAll<GameObject>();

        foreach (var obj in allObjects)
        {
            if (obj.name == "EndRunPanel") deathPanel = obj;
            if (obj.name == "LevelUpPanel") levelUpPanel = obj;
        }

        // Buscar los objetos por nombre (ajustá estos nombres según tu jerarquía)

        if (endRunText == null)
        {
            GameObject endTextObj = GameObject.Find("TimerText");
            if (endTextObj != null)
                endRunText = endTextObj.GetComponent<UnityEngine.UI.Text>();
        }

        if (timerText == null)
        {
            GameObject timerObj = GameObject.Find("TimerText");
            if (timerObj != null)
                timerText = timerObj.GetComponent<UnityEngine.UI.Text>();
        }

        if (deathPanel != null)
        {
            retryButton = deathPanel.transform.Find("RestartButton")?.GetComponent<Button>();
            mainMenuButton = deathPanel.transform.Find("MenuButton")?.GetComponent<Button>();

            if (retryButton != null && mainMenuButton != null)
            {
                retryButton.onClick.RemoveAllListeners();
                mainMenuButton.onClick.RemoveAllListeners();

                retryButton.onClick.AddListener(OnRetryPressed);
                mainMenuButton.onClick.AddListener(OnMainMenuPressed);
            }
        }

        Debug.Log("UIManager: referencias actualizadas para la escena " + SceneManager.GetActiveScene().name);
    }

    public void UpdateTimer(float time)
    {
        this.time = time;
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        if (timerText != null)
            timerText.text = $"{minutes:00}:{seconds:00}";
    }

    public void OnPlayerDeath()
    {
        int survived = (int)(GameManager.Instance.matchDuration - time);
        GameManager.Instance.EndRun(survived);
    }

    public void ShowLevelUpOptions(PlayerController player)
    {
        levelUpPanel.SetActive(true);
        levelUpPanel.GetComponent<LevelUpPopup>().Show(player);
    }

    public void ShowEndRun(int reward)
    {
        if (deathPanel != null)
        {
            deathPanel.SetActive(true);
            Time.timeScale = 0f; // pausa el juego
        }

        if (endRunText != null)
            endRunText.text = $"+{reward} Coins";
    }

    private void OnRetryPressed()
    {
        Debug.Log("Reiniciando nivel...");
        Time.timeScale = 1f;
        SceneManager.LoadScene("Game");
    }

    private void OnMainMenuPressed()
    {
        Debug.Log("Volviendo al menú principal...");
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
