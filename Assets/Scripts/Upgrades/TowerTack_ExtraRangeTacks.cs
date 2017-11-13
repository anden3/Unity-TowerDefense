using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Upgrade_TowerTack_ExtraRangeTacks", menuName = "Upgrades/Tack Tower/Extra Range Tacks")]
public class TowerTack_ExtraRangeTacks : Upgrade {
    public float newAttackRange;

    public override void AddUpgrade(GameObject obj) {
        TowerController tower = obj.GetComponent<TowerController>();
        tower.AttackRange = newAttackRange;
    }
}
