using UnityEngine;
using System.Collections;

public class Respawn: MonoBehaviour {

    Vector3 pos;

    void Start () {
        pos = new Vector3(-2.907191f, 10f, -85f);
    }

    // Update is called once per frame
    void Update () {
        //checks if player is below a certain height;
        //if so, resets their position, velocity, and rotation.
        if(transform.position.y < -10.0) {
            transform.position = pos;
            GetComponent<Rigidbody>().velocity = new Vector3(0,0,0);
            transform.rotation = new Quaternion(0,0,0,0);
        }
    }
}
