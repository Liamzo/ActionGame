using UnityEngine;
using System.Collections;
using System;

public class CrateScript : MonoBehaviour, IDamageable {

    public GameObject destroyedPrefab;

    float health = 1f;

    public void TakeDamage ( float damage ) {
        health -= damage;

        if (health <= 0) {
            Break();
        }
    }

    void OnCollisionEnter ( Collision collision ) {
        if (collision.relativeVelocity.magnitude > 9f) {
            Break();
        }
    }

    void Break () {
        Instantiate(destroyedPrefab, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
