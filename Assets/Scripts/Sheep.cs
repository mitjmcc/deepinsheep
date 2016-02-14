using UnityEngine;
using System.Collections;

public class Sheep : MonoBehaviour {

    public int points;
    Rigidbody body;

	void Start () {
        body = GetComponent<Rigidbody>();
	}
	
	void FixedUpdate () {
        //Check if the sheep is not upright and is not moving
        //Then lerp it back upright it and set any remainging velocities to zero
        if (transform.up != Vector3.up && body.velocity.magnitude < 2f) {
            transform.up = Vector3.Lerp(transform.up, Vector3.up, .25f);
            body.velocity = Vector3.zero;
            body.angularVelocity = Vector3.zero;
        }
	}
}
