using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
    public float speed;
    public int Durability {
        get { return durability; }
        set {
            durability = value;

            if (durability == 0) {
                Destroy(gameObject);
            }
        }
    }

    [SerializeField] private int durability;

    private void OnBecameInvisible() {
        Destroy(gameObject);
    }
}
