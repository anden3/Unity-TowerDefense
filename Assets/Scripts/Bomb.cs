using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : Projectile {
    public float explosionRadius;

    protected override void HitEnemy(Enemy enemy) {
        Collider2D[] matches = Physics2D.OverlapCircleAll(
            transform.position, explosionRadius, LayerMask.GetMask("Enemies")
        );

        foreach (Collider2D match in matches) {
            match.GetComponent<Enemy>().Pop();
        }

        Destroy(gameObject);
        isDestroyed = true;
    }
}
