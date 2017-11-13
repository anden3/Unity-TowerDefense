using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Upgrade_TowerTack_FasterShooting", menuName = "Upgrades/Tack Tower/Faster Shooting")]
public class TowerTack_FasterShooting : Upgrade {
    public float newFireRate;

    public override void AddUpgrade(GameObject obj) {
        TowerController tower = obj.GetComponent<TowerController>();
        tower.fireRate = newFireRate;
    }
}
