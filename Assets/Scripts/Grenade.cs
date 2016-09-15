using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Grenade : MonoBehaviour, IDoesDamage {

    // For DoesDamage
    float lifeTime = 2f;
    float explosionMinRange = 2f;
    float explosionMaxRange = 5f;
    float maxDamage = 10f;
	
	// Update is called once per frame
	void Update () {
        lifeTime -= Time.deltaTime;

        if (lifeTime <= 0) {
            DealDamage();
        }
	}

    public void DealDamage () {
        Collider[] sphereColliders = Physics.OverlapSphere(this.transform.position, explosionMaxRange);
        List<Collider> nonCoverColliders = new List<Collider>();
        List<RaycastHit> hits = new List<RaycastHit>();
        float fallOffRange = explosionMaxRange - explosionMinRange;

        foreach (Collider col in sphereColliders) {
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
                Debug.Log(damage);
            }
        }


        Destroy(this.gameObject);
    }
}
