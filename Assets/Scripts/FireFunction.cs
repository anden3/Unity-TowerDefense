using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FireFunction : ScriptableObject {
    [HideInInspector]
    public bool canFire = true;

    public abstract void Initialize(GameObject obj);
    public abstract IEnumerator Fire(Enemy target);
}
