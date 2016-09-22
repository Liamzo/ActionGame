using UnityEngine;
using System.Collections;
using System;

public class LazerBullet : MonoBehaviour, IDoesDamage {
    float damage = 10f;
    float speed = 30f;
    Rigidbody rb;

    float lifeTime = 10f;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
        transform.position += transform.rotation * new Vector3(0, 1f, 0);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        rb.velocity = transform.rotation * new Vector3(0,speed,0);

        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0) {
            Destroy(gameObject);
        }
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
