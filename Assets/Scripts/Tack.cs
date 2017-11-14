using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tack : Projectile {
    public int durability;

    protected override void HitEnemy(Enemy enemy) {
        enemy.Pop();

        if (--durability == 0) {
            Destroy(gameObject);
            isDestroyed = true;
        }
    }
}
