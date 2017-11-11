using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyExit : MonoBehaviour {
    private void OnTriggerEnter2D(Collider2D collider) {
        GameObject unit = collider.gameObject;

        GameController.instance.Lives -= unit.GetComponent<Enemy>().RBE;
        Destroy(unit);
    }
}
