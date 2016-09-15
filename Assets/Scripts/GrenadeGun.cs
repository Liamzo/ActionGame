using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class GrenadeGun : MonoBehaviour, IShootable, IAimable {

    // For IShootable
    public GameObject grenadeSpawnPoint;

    public GameObject grenade_prefab;
    bool shootReset = true;

    float grenadeImpluse = 17f;

    // For IAimable
    public GameObject uiCrosshair;
    public Sprite crosshair;
    private float _aimDistance = 2f;

	// Update is called once per frame
	void Update () {
        //TODO: make more general, and fix for controllers
	    if (Input.GetMouseButtonUp(0)) {
            shootReset = true;
        }
	}

    public void Shoot () {
        if (shootReset == true) {
            Vector3 dir;

            RaycastHit hit;           
            if (Physics.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Camera.main.transform.forward * 200f, out hit)) {
                dir = (hit.point - grenadeSpawnPoint.transform.position) / hit.distance;
            } else {
                dir = new Vector3(0, 0, 0);
            }

            GameObject grenade = (GameObject)Instantiate(grenade_prefab, grenadeSpawnPoint.transform.position, Camera.main.transform.rotation);
            Rigidbody rb = grenade.GetComponent<Rigidbody>();
            rb.AddForce(dir*grenadeImpluse, ForceMode.Impulse);

            shootReset = false;
        }
    }

    public void StartAim () {
        // Set the crosshair on screen to be this weapon's crosshair
        uiCrosshair.SetActive(true);
        uiCrosshair.GetComponent<Image>().sprite = crosshair;
        Camera.main.GetComponent<MouseLook>().DistanceTarget = _aimDistance;
    }

    public void StopAim () {
        // Set the crosshair on screen to be blank
        uiCrosshair.SetActive(false);
    }
}
