using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerDartFire : MonoBehaviour {
    public GameObject projectile;
    public float fireRate;

    private TowerController tower;

    private void Awake() {
        tower = gameObject.GetComponent<TowerController>();
    }

    private void Start() {
        StartCoroutine(FireControl());
    }

    IEnumerator FireControl() {
        while (true) {
            yield return new WaitUntil(() => tower.target != null);

            Projectile dart = Instantiate(projectile.gameObject, transform.position, transform.rotation).GetComponent<Projectile>();
            Rigidbody2D rb = dart.GetComponent<Rigidbody2D>();

            Vector3 vectorToTarget = tower.target.transform.position - dart.transform.position;
            rb.velocity = vectorToTarget * Time.deltaTime * dart.speed;

            yield return new WaitForSeconds(fireRate);
        }
    }
}
