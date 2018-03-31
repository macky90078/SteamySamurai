using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float spawnRate = 1.0f;
    [SerializeField] private float initialDelay = 1.0f;

    private GameObject gameController;
    private GameManager gameManager;
    private float timer;

	// Use this for initialization
	void Start () {
        gameManager = GameManager.reference;
        timer = initialDelay;
	}
	
	// Update is called once per frame
	void Update () {
        if(gameManager.spawnEnemies == true)
        {
            SpawnEnemy();
        }
	}

    void SpawnEnemy()
    {
        timer -= Time.deltaTime;
        if(timer <= 0)
        {
            timer = spawnRate;
            Instantiate(enemyPrefab, transform.position, transform.rotation);
            gameManager.enemiesSpawned += 1;
        }
    }
}
