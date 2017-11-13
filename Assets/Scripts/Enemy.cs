using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    public float speed;

    public List<string> immunities;
    public GameObject child = null;

    public int RBE;

    private Rigidbody2D rb;

    private void Awake() {
        Enemy current = this;
        RBE = 1;

        while (current.child != null) {
            current = current.child.GetComponent<Enemy>();
            RBE++;
        }

        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if (!(col.tag == "Dart")) {
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

            // Make speed of child be affected by global enemy speed.
            float speed = (rb.velocity.magnitude / this.speed) * e.speed;
            e.GetComponent<Rigidbody2D>().velocity = rb.velocity.normalized * speed;
        }

        GameController.instance.Money += RBE;
        Destroy(gameObject);
    }
}
