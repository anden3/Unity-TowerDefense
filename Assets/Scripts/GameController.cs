using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public static GameController instance = null;

    public int Money {
        get { return money; }
        set {
            money = value;
            moneyText.text = money.ToString();
        }
    }

    public int Lives {
        get { return lives; }
        set {
            lives = value;

            if (lives <= 0) {
                Application.Quit();
            }
            else {
                livesText.text = lives.ToString();
            }
        }
    }

    [Header("Game Properties")]
    [SerializeField] private int money;
    [SerializeField] private int lives;

    [Header("Enemy Settings")]
    public List<Wave> waves;
    public float enemySpawnDelay;
    public Transform enemySpawnPosition;
    public Vector2 enemySpawnDirection;
    public float globalEnemySpeed;

    [Header("UI Elements")]
    public Text waveText;
    public Text moneyText;
    public Text livesText;

    [HideInInspector]
    public bool waveInProgress = false;

    private int waveIndex = 0;

    private void Awake() {
        if (instance == null) {
            instance = this;
        }
        else if (instance != this) {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        waveText.text = "1";
        moneyText.text = money.ToString();
        livesText.text = lives.ToString();
    }

    IEnumerator SpawnWave(Wave wave) {
        foreach (EnemyCount enemies in wave.enemies) {
            for (int i = 0; i < enemies.count; i++) {
                GameObject unit = Instantiate(enemies.type, enemySpawnPosition.position, Quaternion.identity);

                Rigidbody2D rb = unit.GetComponent<Rigidbody2D>();
                Enemy enemy = unit.GetComponent<Enemy>();

                rb.velocity = enemySpawnDirection * enemy.speed * globalEnemySpeed;

                yield return new WaitForSeconds(enemySpawnDelay);
            }
        }

        yield return new WaitUntil(() => GameObject.FindGameObjectsWithTag("Enemy").Length == 0);

        waveInProgress = false;
        waveText.text = (waveIndex + 1).ToString();
        Money += 100 - waveIndex;
    }

    public void StartNextWave() {
        if (waveInProgress || waveIndex >= waves.Count) {
            return;
        }

        waveInProgress = true;
        StartCoroutine(SpawnWave(waves[waveIndex++]));
    }
}
