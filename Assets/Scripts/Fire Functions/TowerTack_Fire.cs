using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerTack_Fire : FireFunction {
    private TowerController tower;

    readonly float[] fireAngles = new float[8] {
        0, 45, 90, 135, 180, 225, 270, 315
    };

    public override void Initialize(GameObject obj) {
        canFire = true;
        tower = obj.GetComponent<TowerController>();
    }

    public override IEnumerator Fire(Enemy target) {
        foreach (float angle in fireAngles) {
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            Projectile tack = Instantiate(
                tower.projectile.gameObject, tower.transform.position, rotation
            ).GetComponent<Projectile>();

            tack.Durability = tower.projectileDurability;
            tack.GetComponent<Rigidbody2D>().velocity = rotation * Vector2.up * tack.speed;
        }

        yield return new WaitForSeconds(tower.fireDelay);

        canFire = true;
    }
}
