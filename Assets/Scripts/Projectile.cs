using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
    public int Durability {
        get { return durability; }
        set {
            durability = value;

            if (durability == 0) {
                Destroy(gameObject);
            }
        }
    }

    public float speed;

    private int durability;
}
