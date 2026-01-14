using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelComplete : MonoBehaviour
{
    public static LevelComplete instance;

    private void Awake()
    {
        instance = this;
    }

    public void EndLevel()
    {
        UnlockNewLevel();
        
        if (PauseMenu.instance != null)
        {
            PauseMenu.instance.ShowLevelCompleteScreen();
        }
    }

    public void UnlockNewLevel() {
        int currentUnlocked = PlayerPrefs.GetInt("UnlockedLevel", 1);
        int currentLevelBuildIndex = SceneManager.GetActiveScene().buildIndex;

        if (currentLevelBuildIndex >= currentUnlocked) {
            PlayerPrefs.SetInt("UnlockedLevel", currentLevelBuildIndex + 1);
            PlayerPrefs.Save();
        }
    }
}
