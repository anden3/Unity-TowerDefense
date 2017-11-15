using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {
    private Animator animator;

    private void Awake() {
        animator = gameObject.GetComponent<Animator>();
    }

    private void Start() {
        StartCoroutine(DestroyWhenAnimationDone());
    }

    private IEnumerator DestroyWhenAnimationDone() {
        AnimatorClipInfo currentClipInfo = animator.GetCurrentAnimatorClipInfo(0)[0];
        yield return new WaitForSeconds(currentClipInfo.clip.length);
        Destroy(gameObject);
    }
}
