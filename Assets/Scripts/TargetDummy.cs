using UnityEngine;
using System.Collections;

public class TargetDummy : MonoBehaviour, IDamageable {

    float maxHealth = 10f;
    public float curHealth = 10f;

    public void TakeDamage (float damage) {
        curHealth -= damage;

        if (curHealth <= 0f) {
            Destroy(this.gameObject);
        }
    }
}
