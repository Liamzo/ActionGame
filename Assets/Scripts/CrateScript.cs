using UnityEngine;
using System.Collections;
using System;

public class CrateScript : MonoBehaviour, IDamageable {

    public GameObject destroyedPrefab;

    float health = 1f;

    public void TakeDamage (float damage) {
        health -= damage;

        if (health <= 0) {
            Instantiate(destroyedPrefab, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
