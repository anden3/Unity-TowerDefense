using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName="Upgrade_TowerDart_LongRangeDarts", menuName="Upgrades/Dart Tower/Long Range Darts")]
public class TowerDart_LongRangeDarts : Upgrade {
    public float newAttackRange;

    public override void AddUpgrade(GameObject obj) {
        TowerController tower = obj.GetComponent<TowerController>();
        tower.AttackRange = newAttackRange;
    }
}
