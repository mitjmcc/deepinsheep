using UnityEngine;

/// <summary>
/// Simple third person controller with sheep wrangling capabilites.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour

{
    public float moveForce;
    public float distance;
    public float hieght;
    public float xRot;
    public float hitStrength;

    int player;
    int playerControl = 1;

    float lookSpeed = 50;
    float x = 0.0f;
    float y = 0.0f;
    float dx;
    float dy;
    float dz;

    Rigidbody body;
    Camera cam;
    Transform model;
    Vector3 velocity;
    Vector3 grav = new Vector3(0, Physics.gravity.y, 0);

    void Start()
    {
        //Initialize the component references
        body = GetComponent<Rigidbody>();
        Debug.Assert(transform.Find("Camera").gameObject.GetComponent<Camera>());
        cam = transform.Find("Camera").gameObject.GetComponent<Camera>();
        Debug.Assert(transform.Find("Model"));
        model = transform.FindChild("Model");
        distance = -cam.transform.localPosition.z;
        hieght = cam.transform.localPosition.y;
    }

    void FixedUpdate()
    {
        //Get move inputs and scale the move speed by the axis values
        dx = Input.GetAxis("Move Vertical " + (player)) * moveForce * playerControl;
        dz = Input.GetAxis("Move Horizontal " + (player)) * moveForce * playerControl;

        //Project camera direction onto xz-plane
        Vector3 camForward = Vector3.Scale(cam.transform.forward, new Vector3(1, 0, 1)).normalized;


        //Create velocity vector and scake it by the speed
        velocity = dx * camForward + dz * cam.transform.right + grav;

        //Set the velocity to the new velocity
        body.velocity = velocity;

        transform.forward = Vector3.Lerp(transform.forward, (dx * camForward + dz * cam.transform.right).normalized, .4f);
    }

    void Update() {
        //Get inputs from the camera inputs and the hit button
        x += Input.GetAxis("Look Horizontal " + (player)) * lookSpeed * distance * 0.02f * playerControl;
        var trig = Input.GetAxis("Hit " + (player));

        //Calculate the rotation using Euler angles,
        //get distance from player, calculate camera position
        Quaternion rotation = Quaternion.Euler(xRot, x, 0);
        Vector3 negDistance = new Vector3(0.0f, hieght, -distance);
        Vector3 position = rotation * negDistance + model.position;

        //Set the camera's new rotation and position
        cam.transform.rotation = rotation;
        cam.transform.position = position;

        //Camera collision
        RaycastHit hit;
        if (Physics.Raycast(model.position,
            (cam.transform.position - model.position).normalized, out hit, distance)
            && hit.transform.tag != "Sheep" && hit.transform.tag != "Terrain")
        {
            //cam.transform.position = hit.point + new Vector3(0, 1f, 0);
        }
        //Player pushes sheep if hit button is pressed
        if (trig > 0)
            hitSheep();
    }

    public void setPlayerNum(int i)
    {
        player = i;
    }

    /// <summary>Set whether the players can move or not</summary>
    /// <param name="i">1 for movement, 0 for no movement</param>
    public void setPlayerControl(int i) {
        playerControl = i;
    }

    /// <summary>
    /// Call when the player is hitting a sheep
    /// </summary>
    void hitSheep() {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, model.forward, out hit, 5))
        {
            if (hit.transform.gameObject.tag == "Sheep")
                hit.transform.gameObject.GetComponent<Sheep>().changeVelocity(model.forward * hitStrength);
        }
    }
}
