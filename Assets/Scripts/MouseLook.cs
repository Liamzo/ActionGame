using UnityEngine;
using System.Collections;

public class MouseLook : MonoBehaviour {

    public Transform target1;
    public float lookSensitivity = 2f;

    public float speed = 2f;

    float moveUpDown = 0f;
    public float upRange = -35f;
    public float downRange = 50f;

    public float distance = 5.0f; // Current distance from target
    public float distanceMin = 0.5f; // Closest cam gets to target
    private float _distanceTarget = 5f;   // The distance the cam is trying to be
    public float DistanceTarget { set { _distanceTarget = value; } }

    public float distanceNormal = 5f; // Distance when just running around

    // Use this for initialization
    void Start () {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetAxisRaw("Fire2") == 0) {
            _distanceTarget = distanceNormal;
        }

        // Move camera up and down
        moveUpDown -= Input.GetAxis("Mouse Y") * lookSensitivity;
        moveUpDown = Mathf.Clamp(moveUpDown, upRange, downRange);
        this.transform.rotation = Quaternion.Euler(moveUpDown, transform.rotation.eulerAngles.y, 0);

        // Move around player
        float moveLeftRight = Input.GetAxis("Mouse X") * lookSensitivity;
        this.transform.RotateAround(target1.position, Vector3.up, moveLeftRight);

        // Follow player and avoid walls
        distance = Mathf.Clamp(distance, distanceMin, _distanceTarget);

        Vector3 positionTarget = new Vector3(0f, 0f, -_distanceTarget);

        // Check if any obstacles are blocking the camera, and if so, move the camera infront of them
        RaycastHit hit;
        if (Physics.Linecast(target1.position, this.transform.rotation * positionTarget + target1.position, out hit)) {
            distance -= (distance - hit.distance);
        } else {
            distance = _distanceTarget;
        }

        // Update position of the camera
        Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
        Vector3 position = this.transform.rotation * negDistance + target1.position;
        transform.position = position;
        //float step = speed * Time.deltaTime;
        //transform.position = Vector3.MoveTowards(transform.position, position, step);

    }
}
