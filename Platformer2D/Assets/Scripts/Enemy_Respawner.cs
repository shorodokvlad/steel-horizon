
using UnityEngine;

public class Enemy_Respawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform[] respawnPoints;
    [SerializeField] private float cooldown= 10f;
    [Header("Spawn Limits")]
    [SerializeField] private int totalEnemiesToSpawn = 10; 
    private int enemiesSpawned = 0;
    private float timer;
    private Transform player;

    private void Awake()
    {
        player = FindFirstObjectByType<Player>().transform;

        timer = 0;
    }
    private void Start()
    {
        if (ObjectiveManager.instance != null)
        {
            ObjectiveManager.instance.AddSpawnedEnemiesToTotal(totalEnemiesToSpawn);
        }
    }
    private void Update()
    {
        if (enemiesSpawned >= totalEnemiesToSpawn)
        {
            return;
        }
        timer -= Time.deltaTime;

        if(timer < 0)
        {
            timer = cooldown;
            CreateNewEnemy();
        }
    }


    private void CreateNewEnemy()
    {
        if (enemiesSpawned >= totalEnemiesToSpawn) return;

        int respawnPointIndex = Random.Range(0, respawnPoints.Length);
        Vector3 spawnPoint = respawnPoints[respawnPointIndex].position;

        GameObject newEnemy = Instantiate(enemyPrefab, spawnPoint, Quaternion.identity);
        
        enemiesSpawned++;

        bool createdOnTheRight = newEnemy.transform.position.x > player.transform.position.x;

        if (createdOnTheRight)
        {
            Enemy enemyScript = newEnemy.GetComponent<Enemy>();
            if (enemyScript != null)
                enemyScript.Flip();
        }
    }
}
