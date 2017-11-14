using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Upgrade : MonoBehaviour {
    public new string name;
    public int cost;

    public abstract void AddUpgrade(GameObject obj);
}
