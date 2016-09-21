using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Grenade : MonoBehaviour, IDoesDamage {

    // Explosion
    bool firstHit = false;
    public GameObject explosion;

    // For DoesDamage
    float lifeTime = 2f;
    float explosionMinRange = 2f;
    float explosionMaxRange = 5f;
    float maxDamage = 10f;

    // For explosion force
    float explosionForce = 5f;
	
	// Update is called once per frame
	void Update () {
        lifeTime -= Time.deltaTime;

        if (lifeTime <= 0) {
            DealDamage(null);
        }
	}

    public void DealDamage (Transform damagedObject) {
        Explode();

        Collider[] sphereColliders = Physics.OverlapSphere(this.transform.position, explosionMaxRange); // Holds all colliders in range
        List<Collider> nonCoverColliders = new List<Collider>(); // Holds all colliders that can be seen
        List<RaycastHit> hits = new List<RaycastHit>(); // Holds hit information for calculating damage
        float fallOffRange = explosionMaxRange - explosionMinRange;

        // Finds the non cover colliders, and stores the raycast information
        foreach (Collider col in sphereColliders) {

            // Checks if collider can be damaged and is out of cover
            RaycastHit hit;
            if (col.transform.GetComponent<IDamageable>() != null && Physics.Raycast(this.transform.position, col.transform.position - this.transform.position, out hit)) {
                if (hit.collider == col) {
                    nonCoverColliders.Add(col);
                    hits.Add(hit);
                }
            }      
        }

        for (int i = 0; i < nonCoverColliders.Count; i++) {
            // If too close to the centre, take the full damage
            if (hits[i].distance <= explosionMinRange) {
                nonCoverColliders[i].transform.GetComponent<IDamageable>().TakeDamage(maxDamage);
            } else {
                // Else, take a lesser amount
                float damageDropOff = ((hits[i].distance - explosionMinRange) / fallOffRange) * maxDamage;
                float damage = maxDamage - damageDropOff;
                nonCoverColliders[i].transform.GetComponent<IDamageable>().TakeDamage(damage);
            }
        }

        // APPLY EXPLOSION FORCE

        // OverlapSphere again to find any new colliders, such as newly broken crates
        sphereColliders = Physics.OverlapSphere(this.transform.position, explosionMaxRange);

        foreach (Collider col in sphereColliders) {
            // Aplies force to all rigidbodies in range
            Rigidbody rb = col.GetComponent<Rigidbody>();
            if (rb != null) {
                rb.AddExplosionForce(explosionForce, transform.position, explosionMaxRange, 3f, ForceMode.Impulse);
            }

        }

        Destroy(gameObject);
    }

    void Explode () {
        Instantiate(explosion, this.transform.position, Quaternion.identity);
    }

    void OnCollisionEnter (Collision collision) {
        if (firstHit == false) {
            if (collision.transform.GetComponent<IDamageable>() != null) {
                DealDamage(null);
            }
        }

        firstHit = true;
    }
}
