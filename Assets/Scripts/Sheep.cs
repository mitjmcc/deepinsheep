using UnityEngine;
using System.Collections;

public class Sheep : MonoBehaviour {

    public int points;
    Rigidbody body;
	AudioSource audioSource;

	void Start () {
        body = GetComponent<Rigidbody>();
		audioSource = GetComponent<AudioSource> ();
	}
	
	void FixedUpdate () {
        //Check if the sheep is not upright and is not moving
        //Then lerp it back upright it and set any remainging velocities to zero
        if (body.velocity.magnitude < 2f && transform.up != Vector3.up) {
            transform.up = Vector3.Lerp(transform.up, Vector3.up, .25f);
            body.velocity = Vector3.zero;
            body.angularVelocity = Vector3.zero;
        }

		//if (Random.Range (0, 1000) > 998) {
		//	baa ();
		//}
	}

    public void changeVelocity(Vector3 velocity)
    {
        body.velocity = velocity;
    }

	void baa() {
		if (!audioSource.isPlaying) {
			audioSource.pitch = (float) 0.1 * Random.Range (10, 16);
			audioSource.Play ();
		}
	}
}
