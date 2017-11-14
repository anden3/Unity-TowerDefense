using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerDart_PiercingDarts : Upgrade {
    public int newProjectileDurability;

    public override void AddUpgrade(GameObject obj) {
        TowerController tower = obj.GetComponent<TowerController>();
        tower.projectileDurability = newProjectileDurability;
    }
}