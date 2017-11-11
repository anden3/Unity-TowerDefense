using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    public int value;
    public int health;
    public float speed;

    public List<string> immunities;
    public GameObject child;
}
