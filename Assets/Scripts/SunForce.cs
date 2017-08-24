using UnityEngine;
using System.Collections;

public class SunForce : MonoBehaviour {
    
    public Rigidbody[] rb;

	// Use this for initialization
	void Start () {
        
    }
	
	
	void FixedUpdate () {
        if (rb[0])
        {
            for(int i = 0; i < rb.Length; i++)
            {
                rb[i].AddForce(transform.position - rb[i].transform.position);
            }
        }
	}
}
