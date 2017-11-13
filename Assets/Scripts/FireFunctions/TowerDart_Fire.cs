using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="FireFunction_TowerDart", menuName="Fire Functions/Dart Tower")]
public class TowerDart_Fire : FireFunction {
    private TowerController tower;

    public override void Initialize(GameObject obj) {
        tower = obj.GetComponent<TowerController>();
    }

    public override IEnumerator Fire(Enemy target) {
        canFire = false;

        Projectile dart = Instantiate(
            tower.projectile.gameObject, tower.transform.position, tower.transform.rotation
        ).GetComponent<Projectile>();

        dart.Durability = tower.projectileDurability;
        Vector3 vectorToTarget = target.transform.position - dart.transform.position;
        dart.GetComponent<Rigidbody2D>().velocity = vectorToTarget * Time.deltaTime * dart.speed;

        yield return new WaitForSeconds(tower.fireRate);

        canFire = true;
    }
}
