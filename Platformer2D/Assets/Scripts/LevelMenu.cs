using UnityEngine;
using UnityEngine.UI;
using TMPro; 

public class LevelMenu : MonoBehaviour
{
    [Header("UI References")]
    public Image displayImage;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI objectiveText;
    
    [Header("Level Configuration")]
    public LevelData[] levels;
    public Button[] levelButtons;

    private int selectedLevelIndex = 0;

    [System.Serializable]
    public class LevelData {
        public string levelName;
        public string zoneTitle; 
        [TextArea] public string description;
        public string objective;
        public Sprite previewImage;
    }

    private void Awake()
    {
        // TEMPORARY: Uncomment this line once, run the build, 
        // then comment it out and rebuild to ensure a clean state.
        //PlayerPrefs.DeleteAll(); 

        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);

        for (int i = 0; i < levelButtons.Length; i++)
        {
            // Lock buttons if level isn't reached yet
            levelButtons[i].interactable = (i < unlockedLevel);
            
            // We need to capture the index for the listener
            int index = i; 
            levelButtons[i].onClick.AddListener(() => SelectLevel(index));
        }
        SelectLevel(0);
    }

    public void SelectLevel(int index)
    {
        selectedLevelIndex = index;

        displayImage.sprite = levels[index].previewImage;
        titleText.text = levels[index].zoneTitle;
        descriptionText.text = levels[index].description;
        objectiveText.text = levels[index].objective;
    }

    public void PlaySelectedLevel()
    {
        string sceneToLoad = levels[selectedLevelIndex].levelName;
        SceneController.instance.LoadScene(sceneToLoad);
    }
}