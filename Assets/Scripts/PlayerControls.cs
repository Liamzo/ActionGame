using UnityEngine;
using System.Collections;

public class PlayerControls : MonoBehaviour {

    CharacterController cc;
    Camera cam;
    
    // Movement
    public float walkMoveSpeed = 7f;
    public float airMoveSpeed = 3.5f;
    public float curMoveSpeed = 7f;
    public float jumpSpeed = 9f;
    public float verticalVelocity = 0f;
    int jumpsLeft = 2;

    Vector3 lastGroundMovement;

    // Shooting
    public GameObject activeWeapon;
    bool shouldLookAtCamForward = false;

    // Use this for initialization
    void Start () {
        cc = GetComponent<CharacterController>();
        cam = Camera.main;
    }
	
	// Update is called once per frame
	void Update () {
        // MOVEMENT

        // Set the move speed depending on current position action
        if (cc.isGrounded == false) {
            curMoveSpeed = airMoveSpeed;
        } else {
            curMoveSpeed = walkMoveSpeed;
        }

        // Check player input
        float forwardSpeed = Input.GetAxis("Vertical") * curMoveSpeed;
        float sideSpeed = Input.GetAxis("Horizontal") * curMoveSpeed;

        // Set the movement to be in the direction of the camera
        Vector3 movement = cam.transform.rotation * new Vector3(sideSpeed, 0, forwardSpeed);
        movement.y = 0;

        // Reset veritcal speed and jumps if on the ground, or increase downward speed
        if (cc.isGrounded) {
            verticalVelocity = -0.1f;
            jumpsLeft = 2;
        } else {
            verticalVelocity += Physics.gravity.y * 2f * Time.deltaTime;
        }

        // If we can jump, set the vertical speed and decrease remaining jumps
        if (Input.GetButtonDown("Jump") && jumpsLeft > 0) {
            verticalVelocity = jumpSpeed;
            jumpsLeft--;
            lastGroundMovement = movement;
        }

        // CHANGE ROTATION
        // Only change character rotation if moving and on the ground
        if (movement != Vector3.zero && cc.isGrounded && shouldLookAtCamForward == false) {
            transform.LookAt(transform.position + movement.normalized);
        } else if (shouldLookAtCamForward == true) {
            lookAtCamForward();
        }

        movement.y = verticalVelocity;

        // Only slowly adjust in air movement
        // COULD DO: Set lastGroundMovement to current movement to allow actual change
        if (!cc.isGrounded) {
            movement += lastGroundMovement;
        }

        cc.Move(movement * Time.deltaTime);

        // SHOOTING

        // Check if the player is holding down the attack button
        if (Input.GetAxisRaw("Fire1") == 1) {
            // Check player has a weapon that can shoot
            if (activeWeapon != null && activeWeapon.GetComponent<IShootable>() != null) {
                lookAtCamForward();
                activeWeapon.GetComponent<IShootable>().Shoot();
            }
        }

        // Check if the player is trying to aim
        if (Input.GetAxisRaw("Fire2") == 1) {
            if (activeWeapon != null && activeWeapon.GetComponent<IAimable>() != null) {
                lookAtCamForward();
                activeWeapon.GetComponent<IAimable>().StartAim();
            }
        } else {
            if (activeWeapon != null && activeWeapon.GetComponent<IAimable>() != null) {
                activeWeapon.GetComponent<IAimable>().StopAim();
            }
        }
    }

    void lookAtCamForward () {
        Vector3 movement = cam.transform.rotation * new Vector3(0, 0, 1);
        movement.y = 0;

        transform.LookAt(transform.position + movement.normalized);
    }
}
