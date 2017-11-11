using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
    public Vector2 spawnDirection = new Vector2(1.0f, 0.0f);
    public GameObject enemyBaseClass;

    public GameObject Spawn(string type) {
        return (GameObject)(Instantiate(Resources.Load(type), transform.position, Quaternion.identity));
    }
}
