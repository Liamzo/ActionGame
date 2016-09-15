using UnityEngine;
using System.Collections;
using System;

public class Grenade : MonoBehaviour, IDoesDamage {

    // For DoesDamage
    float lifeTime = 2f;
    float explosionMinRange = 2f;
    float explosionMaxRange = 5f;
    float maxDamage = 10f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        lifeTime -= Time.deltaTime;

        if (lifeTime <= 0) {
            DealDamage();
        }
	}

    public void DealDamage () {
        Collider[] cols = Physics.OverlapSphere(this.transform.position, explosionMaxRange);

        float fallOffRange = explosionMaxRange - explosionMinRange;

        foreach (Collider col in cols) {
            if (col.transform.GetComponent<IDamageable>() != null) {
                // Check if the object is behind cover
                // TODO: bit weird if the cover is destroyed
                RaycastHit hit;
                if (Physics.Raycast(this.transform.position, col.transform.position - this.transform.position, out hit)) {
                    if (hit.collider != col) {
                        Debug.Log("Cover");
                        break;
                    }

                    // If too close to the centre, take the full damage
                    if (hit.distance <= explosionMinRange) {
                        col.transform.GetComponent<IDamageable>().TakeDamage(maxDamage);
                    } else {
                        // Else, take a lesser amount
                        float damageDropOff = ((hit.distance - explosionMinRange) / fallOffRange) * maxDamage;
                        float damage = maxDamage - damageDropOff;
                        col.transform.GetComponent<IDamageable>().TakeDamage(damage);
                        Debug.Log(damage);
                  }
                }

                
            }
        }

        Destroy(this.gameObject);
    }
}
