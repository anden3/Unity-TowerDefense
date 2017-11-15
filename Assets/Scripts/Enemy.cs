using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    public float speed;
    public float popDisplayTime;
    public GameObject child = null;

    public Sprite popSprite;
    public List<string> immunities;

    [HideInInspector] public int RBE;

    private bool popped = false;
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
        if (popped) {
            return;
        }

        popped = true;
        rb.velocity = new Vector2(0, 0);

        if (child != null) {
            Enemy e = Instantiate(child, transform.position, transform.rotation).GetComponent<Enemy>();

            // Make speed of child be affected by global enemy speed.
            float childSpeed = (rb.velocity.magnitude / speed) * e.speed;
            e.GetComponent<Rigidbody2D>().velocity = rb.velocity.normalized * childSpeed;
        }

        GameController.instance.Money += RBE;
        AudioController.instance.PlayPopSound();

        StartCoroutine(DisplayPopSprite());
    }

    private IEnumerator DisplayPopSprite() {
        SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>();
        sr.sprite = popSprite;
        sr.color = new Color(1, 1, 1, 1);

        yield return new WaitForSeconds(popDisplayTime);

        Destroy(gameObject);
    }
}
