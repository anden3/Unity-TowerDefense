using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionChanger : MonoBehaviour {
    public Vector2 newDirection;
    public float maxDistance;

    private float sqrMaxDistance;

    private void Awake() {
        sqrMaxDistance = Mathf.Pow(maxDistance, 2);
    }

    private void OnTriggerEnter2D(Collider2D unit) {
        if (unit.CompareTag("Enemy")) {
            StartCoroutine(ChangeDirection(unit.gameObject));
        }
    }

    IEnumerator ChangeDirection(GameObject unit) {
        while (true) {
            float sqrDist = (transform.position - unit.transform.position).sqrMagnitude;

            if (sqrDist < sqrMaxDistance) {
                break;
            }

            yield return null;
        }

        Rigidbody2D rb = unit.GetComponent<Rigidbody2D>();
        rb.position = transform.position;
        rb.velocity = rb.velocity.magnitude * newDirection;
    }
}
