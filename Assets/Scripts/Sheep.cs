using UnityEngine;
using System.Collections;

public class Sheep : MonoBehaviour {

    public int points;
    Animation anim;
    Rigidbody body;
	AudioSource audioSource;
    Agent agent;
    State state;
    bool hit;
    int chance;

	void Start () {
        body = GetComponent<Rigidbody>();
		audioSource = GetComponent<AudioSource> ();
        state = State.GRAZE;
        chance = Random.Range(1, 8);
        chance = 100 - chance;
        agent = GetComponent<Agent>();
        anim = GetComponentInChildren<Animation>();
	}
	
	void FixedUpdate () {
        //Check if the sheep is not upright and is not moving
        //Then lerp it back upright it and set any remainging velocities to zero
        if (body.velocity.magnitude < 2f && transform.up != Vector3.up) {
            transform.up = Vector3.Lerp(transform.up, Vector3.up, .25f);
            body.velocity = Vector3.zero;
            body.angularVelocity = Vector3.zero;
            state = State.GRAZE;
        }
        updateState();
        if (Random.Range (0, 1000) > 998) {
			baa();
		}
	}

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Player")
        {
            state = State.HIT;
        }
    }

    public void changeVelocity(Vector3 velocity) {
        state = State.HIT;
        body.velocity = velocity;
    }

	void baa() {
		if (!audioSource.isPlaying) {
			audioSource.pitch = (float) 0.1 * Random.Range (10, 16);
			audioSource.Play ();
		}
	}

    void updateState() {
        switch(state) {
            case State.GRAZE:
                //Set the jumping animation to play only once more
                anim.GetClip("SheepJump").wrapMode = WrapMode.Once;
                //Re-enable the nav mesh agent in case it had been disabled by being hit
                GetComponent<NavMeshAgent>().enabled = true;
                //Chance to change to wander
                //Debug.Log("is grazing");
                if (Random.Range(1, 100) > chance) {
                    //Debug.Log("goes to wander");
                    state = State.WANDER;
                }
                break;
            case State.WANDER:
                //Set the jump animation to loop
                anim.GetClip("SheepJump").wrapMode = WrapMode.Loop;
                anim.Play();
                //Re-enable the nav mesh agent in case it had been disabled by being hit
                GetComponent<NavMeshAgent>().enabled = true;
                //Chance to change back to grazing
                agent.randomLocation();
                if (Random.Range(1, 100) > (chance / 2)) {
                    state = State.GRAZE;
                }
                break;
            case State.HIT:
                //Set the jumping animation to play only once more
                anim.GetClip("SheepJump").wrapMode = WrapMode.Once;
                //Debug.Log("Sheep was hit");
                GetComponent<NavMeshAgent>().enabled = false;
                break;
            default:
                //Re-enable the nav mesh agent in case it had been disabled by being hit
                GetComponent<NavMeshAgent>().enabled = true;
                //Make sheep graze
                state = State.GRAZE;
                break;
        }
    }

    private enum State {WANDER, GRAZE, RUN, HIT};
}
