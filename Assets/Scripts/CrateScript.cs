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

    // Had some issues with synchronization when getting hit and taking damage at the same time
    // Break was being called twice

    // Also annoying with debris colliding with enough force to break it

    void OnCollisionEnter ( Collision collision ) {
        if (collision.relativeVelocity.magnitude > 12f) {
            Break();
        }
    }

    void Break () {
        GameObject go = (GameObject)Instantiate(destroyedPrefab, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
