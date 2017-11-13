using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadiusTrigger : MonoBehaviour {
    private TowerController tower;

    private void Awake() {
        tower = transform.parent.gameObject.GetComponent<TowerController>();
    }
    
    private void OnTriggerEnter2D(Collider2D collider) {
        if (!collider.CompareTag("Enemy")) {
            return;
        }

        tower.EnemySpotted(collider.GetComponent<Enemy>());
    }

    private void OnTriggerExit2D(Collider2D collider) {
        switch (collider.tag) {
            case "Enemy":
                tower.EnemyOutOfRange(collider.GetComponent<Enemy>());
                break;
            
            // Destroy projectiles leaving attack range.
            case "Projectile":
                Destroy(collider.gameObject);
                break;
        }
    }
}
