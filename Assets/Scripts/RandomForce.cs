using UnityEngine;
using System.Collections;

public class RandomForce : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = Random.onUnitSphere * 100;
	}
	
	
}
