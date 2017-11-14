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

    public void Pop() {
        if (child != null) {
            Enemy e = Instantiate(child, transform.position, transform.rotation).GetComponent<Enemy>();

            // Make speed of child be affected by global enemy speed.
            float childSpeed = (rb.velocity.magnitude / speed) * e.speed;
            e.GetComponent<Rigidbody2D>().velocity = rb.velocity.normalized * childSpeed;
        }

        GameController.instance.Money += RBE;
        Destroy(gameObject);
    }
}
