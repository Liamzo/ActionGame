using UnityEngine;
using System.Collections;
using System;

public class LazerBullet : MonoBehaviour, IDoesDamage {
    float damage = 10f;
    float speed = 15f;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.Translate(Vector3.up * Time.deltaTime * speed);
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
