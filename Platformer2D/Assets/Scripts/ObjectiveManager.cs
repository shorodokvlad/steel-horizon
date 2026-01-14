using UnityEngine;
using TMPro;

public class ObjectiveManager : MonoBehaviour
{
    public static ObjectiveManager instance;
    private bool isAllEnemiesKilled = false;
    private bool isKeyRetreived = false;

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI killText;
    [SerializeField] private TextMeshProUGUI keyText;

    private int totalEnemies;
    private int currentKills;
    private int totalKeys;
    private int currentKeys;

    private void Awake()
    {
        if (instance == null) 
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        totalEnemies = 0;
        currentKills = 0;
    }

    private void Start()
    {
        int initialEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length;
        totalEnemies += initialEnemies;

        totalKeys = GameObject.FindGameObjectsWithTag("Key").Length;

        if (totalKeys == 0)
        {
            isKeyRetreived = true;
        }
        UpdateUI();
    }
    
    
    public void AddSpawnedEnemiesToTotal(int amount)
    {
        totalEnemies += amount;
        UpdateUI();
    }
    

    public void UpdateKillCount()
    {
        currentKills++;
        UpdateUI();
        
        if (currentKills >= totalEnemies)
        {
            isAllEnemiesKilled = true;
            ObjectiveCompleted();
        }
    }

    public void UpdateKeyCount()
    {
        currentKeys++;
        UpdateUI();

        if (currentKeys >= totalKeys)
        {
            isKeyRetreived = true;
            ObjectiveCompleted();
        }
    }

    public void ObjectiveCompleted()
    {
        if (isAllEnemiesKilled && isKeyRetreived)
        {
            if (LevelComplete.instance != null)
            {
                LevelComplete.instance.EndLevel();
            } 
        }
    }
    private void UpdateUI()
    {
        if (killText != null)
        {
            killText.text = currentKills + " / " + totalEnemies;
        }

        if (keyText != null)
        {
            keyText.text = currentKeys + " / " + totalKeys;
        }
    }
}