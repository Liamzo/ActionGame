using UnityEngine;
using System.Collections;
using System;

public class BasicShooter : MonoBehaviour, IDamageable {

    // For IDamageable
    float maxHealth = 20f;
    public float curHealth;

    // Controls what actions we take
    enum ShooterState {
        NotInCombat,
        Repositioning,
        Firing
    };
    ShooterState shooterState = ShooterState.NotInCombat;

    public GameObject player;

    // For checking if in sight
    public GameObject playerHead;
    public GameObject ownHead;
    float rangeOfSight = 15f;

    // For firing at the player
    float timeBetweenShots = 1f;
    float curTimeBetweenShots;
    int maxNumberOfShots = 5;
    int minNumberOfShots = 3;
    int shotsRemaining = 0;
    

    // Use this for initialization
    void Start () {
        curHealth = maxHealth;
        curTimeBetweenShots = timeBetweenShots;
	}
	
	// Update is called once per frame
	void Update () {
	    if (shooterState == ShooterState.NotInCombat) {
            // TODO: Maybe patrol an area, etc

            // Check if we can see the player
            RaycastHit hit;
            if (Physics.Raycast(ownHead.transform.position, playerHead.transform.position - ownHead.transform.position, out hit, rangeOfSight)) {
                if (hit.transform.tag == "Player") {
                    shooterState = ShooterState.Firing;
                }
            }
        } else {
            if (shooterState == ShooterState.Repositioning) {
                // TODO: Move to a random location
                Debug.Log("Repositioning");
                shooterState = ShooterState.Firing;
            } else if (shooterState == ShooterState.Firing) {
                // Continue to fire at the player
                lookAt(player.transform.position);

                curTimeBetweenShots -= Time.deltaTime;
                if (curTimeBetweenShots <= 0) {
                    // Fire the gun and reset the timer
                    fire();
                    curTimeBetweenShots = timeBetweenShots;
                }

                if (shotsRemaining <= 0) {
                    // No shots remaining, so start moving
                    shooterState = ShooterState.Repositioning;

                    // Reset the number of shots
                    shotsRemaining = UnityEngine.Random.Range(minNumberOfShots, maxNumberOfShots + 1);
                }
            }
        }
	}

    public void TakeDamage (float damage) {
        curHealth -= damage;

        if (curHealth <= 0) {
            Destroy(gameObject);
        }
    }

    void lookAt(Vector3 target) {
        transform.LookAt(target);
        transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y, transform.eulerAngles.z);
    }

    void fire () {
        Debug.Log("pew");
        shotsRemaining--;
    }

}
