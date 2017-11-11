using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyCount {
    public GameObject type;
    public int count;
}

[System.Serializable]
public class Wave {
    public List<EnemyCount> enemies;
}

public class GameController : MonoBehaviour {
    public List<Wave> waves;
    public float enemySpawnDelay;

    public Transform enemySpawnPosition;
    public Vector2 enemySpawnDirection;

    private int waveIndex = 0;
    private bool startNextWave = true;
    
	void Start () {

	}
	
	void Update () {
		if (startNextWave && waveIndex < waves.Count) {
            startNextWave = false;

            Wave wave = waves[waveIndex];
            waveIndex++;

            StartCoroutine(SpawnWave(wave));
        }
	}

    IEnumerator SpawnWave(Wave wave) {
        foreach (EnemyCount enemies in wave.enemies) {
            for (int i = 0; i < enemies.count; i++) {
                GameObject unit = Instantiate(enemies.type, enemySpawnPosition.position, Quaternion.identity);

                Rigidbody2D rb = unit.GetComponent<Rigidbody2D>();
                Enemy enemy = unit.GetComponent<Enemy>();

                rb.velocity = enemySpawnDirection * enemy.speed;

                yield return new WaitForSeconds(enemySpawnDelay);
            }
        }
    }
}
