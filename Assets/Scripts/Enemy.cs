using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    public int value;
    public int health;
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
}
