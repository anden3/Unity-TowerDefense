using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerDart_Fire : FireFunction {
    private TowerController tower;

    public override void Initialize(GameObject obj) {
        canFire = true;
        tower = obj.GetComponent<TowerController>();
    }

    public override IEnumerator Fire(Enemy target) {
        Dart dart = Instantiate(
            tower.projectile.gameObject, tower.transform.position, tower.transform.rotation
        ).GetComponent<Dart>();

        dart.durability = tower.projectileDurability;

        Vector3 vectorToTarget = target.transform.position - dart.transform.position;
        dart.GetComponent<Rigidbody2D>().velocity = vectorToTarget * dart.speed;

        yield return new WaitForSeconds(tower.fireDelay);

        canFire = true;
    }
}
