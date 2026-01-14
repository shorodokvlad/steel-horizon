using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu instance;
    public static bool isPaused = false;
    public static bool isGameOver = false;
    public static bool isLevelComplete = false;

    public static bool shouldOpenLevels = false;

    [Header("UI Panels")]
    [SerializeField] private GameObject pauseMenuUI;   
    [SerializeField] private GameObject settingsPanel;   
    [SerializeField] private GameObject levelsPanel; 
    [SerializeField] private GameObject gameOverPanel;      
    [SerializeField] private GameObject levelCompletePanel;      

    private void Awake()
    {
        isGameOver = false;
        isLevelComplete = false;
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        Time.timeScale = 1f;
        isPaused = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isGameOver || isLevelComplete) return;

            if (isPaused)
            {
                // If a sub-panel is open, ESC should take you back to the main pause menu
                if (settingsPanel.activeSelf || levelsPanel.activeSelf)
                    OpenPauseMenu();
                else
                    Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    private void Start()
    {
        if (shouldOpenLevels)
        {
            shouldOpenLevels = false;

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            OpenLevels(); 
        }
    }

    public void ShowGameOverScreen()
    {
        isGameOver = true;
        isPaused = true;
        Time.timeScale = 0f;

        if (gameOverPanel != null) gameOverPanel.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void ShowLevelCompleteScreen()
    {
        StartCoroutine(LevelCompleteRoutine());
    }

    private IEnumerator LevelCompleteRoutine()
    {
        yield return new WaitForSecondsRealtime(1.5f);

        isLevelComplete = true;
        isPaused = true;

        Time.timeScale = 0f;

        if (levelCompletePanel != null)
            levelCompletePanel.SetActive(true);
            
        

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    // --- Core Pause Logic ---
    public void Resume()
    {
        SetAllPanelsActive(false);
        Time.timeScale = 1f; 
        isPaused = false;
        
        // Return cursor control to the game
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Pause()
    {
        OpenPauseMenu();
        Time.timeScale = 0f; 
        isPaused = true;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    // --- Sub-Menu Navigation ---
    public void OpenPauseMenu()
    {
        SetAllPanelsActive(false);
        pauseMenuUI.SetActive(true);
    }

    public void OpenSettings()
    {
        SetAllPanelsActive(false);
        settingsPanel.SetActive(true);
    }

    public void OpenLevels()
    {
        SetAllPanelsActive(false);
        levelsPanel.SetActive(true);
    }

    public void OpenLevelsWhenLevelComplete()
    {
        shouldOpenLevels = true; 
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void SetAllPanelsActive(bool state)
    {
        if (pauseMenuUI != null) pauseMenuUI.SetActive(state);
        if (settingsPanel != null) settingsPanel.SetActive(state);
        if (levelsPanel != null) levelsPanel.SetActive(state);
        if (gameOverPanel != null) gameOverPanel.SetActive(state);
        if (levelCompletePanel != null) levelCompletePanel.SetActive(state);
    }

    public void Restart()
    {
        isGameOver = false;
        isPaused = false;

        Time.timeScale = 1f;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitToMainMenu()
    {
        Time.timeScale = 1f;
        isPaused = false;
        SceneManager.LoadScene("MainMenu"); 
    }
}