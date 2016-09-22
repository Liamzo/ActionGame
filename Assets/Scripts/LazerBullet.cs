using UnityEngine;
using System.Collections;
using System;

public class LazerBullet : MonoBehaviour, IDoesDamage {
    float damage = 10f;
    float speed = 15f;
    Rigidbody rb;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        rb.velocity = new Vector3(0,speed,0);
    }

    public void DealDamage (Transform obj) {
        obj.GetComponent<IDamageable>().TakeDamage(damage);
    }

    void OnCollisionEnter (Collision collision) {
        if (collision.transform.GetComponent<IDamageable>() != null) {
            DealDamage(collision.transform);
        }
        Destroy(gameObject);
    }

}
