using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FireFunction : MonoBehaviour {
    [HideInInspector]
    public bool canFire;

    public abstract void Initialize(GameObject obj);
    public abstract IEnumerator Fire(Enemy target);
}
