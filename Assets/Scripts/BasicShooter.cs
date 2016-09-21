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
    public GameObject lazerGun;
    public GameObject lazerBulletPrefab;

    // For randomly moving and repositioning
    NavMeshAgent nma;
    float moveRange = 8f;
    Vector3 targetMovePosition;
    bool findNewPosition = true;

    // Use this for initialization
    void Start () {
        curHealth = maxHealth;
        curTimeBetweenShots = timeBetweenShots;

        shotsRemaining = UnityEngine.Random.Range(minNumberOfShots, maxNumberOfShots + 1);

        nma = GetComponent<NavMeshAgent>();
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
                if (findNewPosition == true) {
                    targetMovePosition = FindRandomPoint(transform.position);
                    Debug.Log(targetMovePosition);
                    findNewPosition = false;
                }
        
                nma.destination = targetMovePosition;
                if (Vector3.Distance(transform.position, targetMovePosition) <= 0.5f) {
                    findNewPosition = true;
                    shooterState = ShooterState.Firing;

                }
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
        lazerGun.transform.LookAt(playerHead.transform.position);
        lazerGun.transform.localEulerAngles = new Vector3(lazerGun.transform.eulerAngles.x, 0f, 0f);
    }

    void fire () {
        // Make a lazer appear
        GameObject go = (GameObject)Instantiate(lazerBulletPrefab, lazerGun.transform.position, Quaternion.Euler(lazerGun.transform.eulerAngles.x+90, lazerGun.transform.eulerAngles.y, lazerGun.transform.eulerAngles.z));
        shotsRemaining--;
    }

    Vector3 FindRandomPoint (Vector3 center) {
        Vector3 result;
        for (int i = 0; i < 30; i++) {
            Vector3 randomPoint = center + UnityEngine.Random.insideUnitSphere * moveRange ;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas)) {
                result = hit.position;
                return result;
            }
        }
        result = Vector3.zero;
        return result;
    }

}
