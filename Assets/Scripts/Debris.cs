using UnityEngine;
using System.Collections;

public class Debris : MonoBehaviour {
  
	void Start () {
        Destroy(gameObject, 20f);
	}

}
