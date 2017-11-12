using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    public float speed;

    public List<string> immunities;
    public GameObject child = null;

    public int RBE {
        get {
            Enemy current = this;
            rbe = 1;

            while (current.child != null) {
                current = current.child.GetComponent<Enemy>();
                rbe++;
            }

            return rbe;
        }
    }
    private int rbe;

    private void OnTriggerEnter2D(Collider2D col) {
        if (!col.CompareTag("Items")) {
            return;
        }

        // Prevent dart from hitting several enemies.
        Projectile d = col.GetComponent<Projectile>();

        if (d.destroyed) {
            return;
        }

        d.destroyed = true;
        Destroy(col.gameObject);

        if (child != null) {
            Enemy e = Instantiate(child, transform.position, transform.rotation).GetComponent<Enemy>();
            e.GetComponent<Rigidbody2D>().velocity = gameObject.GetComponent<Rigidbody2D>().velocity.normalized * e.speed;
        }

        Destroy(gameObject);
    }
}
