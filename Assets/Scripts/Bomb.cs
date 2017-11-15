using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : Projectile {
    public Explosion explosion;
    public float explosionRadius;

    protected override void HitEnemy(Enemy enemy) {
        Collider2D[] matches = Physics2D.OverlapCircleAll(
            transform.position, explosionRadius, LayerMask.GetMask("Enemies")
        );

        foreach (Collider2D match in matches) {
            match.GetComponent<Enemy>().Pop();
        }

        Explosion ex = Instantiate(explosion, transform.position, Quaternion.identity);
        ex.transform.localScale = new Vector3(explosionRadius * 2, explosionRadius * 2, 1);

        Destroy(gameObject);
        isDestroyed = true;
    }
}
