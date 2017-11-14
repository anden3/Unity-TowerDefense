using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSuper_EpicRange : Upgrade {
    public float newAttackRange;

    public override void AddUpgrade(GameObject obj) {
        TowerController tower = obj.GetComponent<TowerController>();
        tower.AttackRange = newAttackRange;
    }
}
