using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : MonoBehaviour {
    public float speed;

    protected bool isDestroyed = false;

    private void OnTriggerEnter2D(Collider2D col) {
        if (isDestroyed || col.tag != "Enemy") {
            return;
        }

        HitEnemy(col.GetComponent<Enemy>());
    }

    protected abstract void HitEnemy(Enemy enemy);
}
