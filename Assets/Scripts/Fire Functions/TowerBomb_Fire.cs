using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="TowerBomb", menuName="Fire Functions/Bomb Tower")]
public class TowerBomb_Fire : FireFunction {
    private TowerController tower;

    public override void Initialize(GameObject obj) {
        canFire = true;
        tower = obj.GetComponent<TowerController>();
    }

    public override IEnumerator Fire(Enemy target) {
        Projectile bomb = Instantiate(
            tower.projectile.gameObject, tower.transform.position, tower.transform.rotation
        ).GetComponent<Projectile>();

        Vector3 vectorToTarget = target.transform.position - bomb.transform.position;
        bomb.GetComponent<Rigidbody2D>().velocity = vectorToTarget * bomb.speed;

        yield return new WaitForSeconds(tower.fireDelay);

        canFire = true;
    }
}
