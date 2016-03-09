using UnityEngine;

public class Sheep : MonoBehaviour {

    public int points;
    public float radius = 2;
    Animation anim;
    Rigidbody body;
	AudioSource audioSource;
    Agent agent;
    State state;
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
            ReErect();
            if (IsPlayerNear())
                state = State.RUN;
            else
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

    void ReErect()
    {
        transform.up = Vector3.Lerp(transform.up, Vector3.up, .25f);
        body.velocity = Vector3.zero;
        body.angularVelocity = Vector3.zero;
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
                if(IsPlayerNear())
                {
                    state = State.RUN;
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
                if (IsPlayerNear())
                {
                    state = State.RUN;
                }
                break;
            case State.HIT:
                //Set the jumping animation to play only once more
                anim.GetClip("SheepJump").wrapMode = WrapMode.Once;
                //Debug.Log("Sheep was hit");
                GetComponent<NavMeshAgent>().enabled = false;
                break;
            case State.RUN:
                anim.Play("SheepRun");
                //Debug.Log("I'm scared!");
                if (Random.Range(1, 100) > (chance / 2) && !IsPlayerNear())
                {
                    state = State.GRAZE;
                }
                break;
            default:
                //Re-enable the nav mesh agent in case it had been disabled by being hit
                GetComponent<NavMeshAgent>().enabled = true;
                //Make sheep graze
                state = State.GRAZE;
                break;
        }
    }

    bool IsPlayerNear()
    {
        RaycastHit hit;
        if (Physics.SphereCast(transform.position, radius, Vector3.forward, out hit, radius))
        {
            if (hit.transform.gameObject.tag == "Player")
            {
                return true;
            }
        }
        return false;
    }

    private enum State {WANDER, GRAZE, RUN, HIT};
}
