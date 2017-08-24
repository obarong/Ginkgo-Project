using UnityEngine;
using System.Collections;

public class RandomForce : MonoBehaviour {

    public float force = 100f;

	// Use this for initialization
	void Start () {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = Random.onUnitSphere * force;
	}
	
	
}
