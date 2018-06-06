using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeCamera : MonoBehaviour {

    public float lookSpeed;
    public float moveSpeed;
    public float excavationSpeed;
    public Terrain Ter;

    private float horizontalAngle;
    private float verticalAngle;

    private const int EXCAVATE_SIZE = 15;
    private const int EXCAVATE_SIZE_2 = EXCAVATE_SIZE / 2;
    private const float LOOK_LIMIT = 70;

    // Use this for initialization
    void Start () {}
	
	// Update is called once per frame
	void Update () {
        // FREE CAMERA LOOK
        horizontalAngle += Input.GetAxis("Mouse X") * lookSpeed;
        verticalAngle += Input.GetAxis("Mouse Y") * lookSpeed;

        constrainCamera();
        Quaternion q = Quaternion.AngleAxis(horizontalAngle, Vector3.up) * Quaternion.AngleAxis(verticalAngle, -Vector3.right);
        transform.rotation = q;

        // MOVEMENT
        Vector3 translation = new Vector3
        {
            x = Input.GetAxis("Horizontal"),
            y = Input.GetAxis("Jump"),
            z = Input.GetAxis("Vertical")
        };
        transform.Translate(translation * moveSpeed);

        // FOG based on depth
        //RenderSettings.fogDensity = 0.045f + Mathf.Max(10.0f - transform.position.y, 0.0f)*0.05f;
    }

    void constrainCamera()
    {
        // make horizontal angle be in range of 360 degrees
        if (horizontalAngle > 360.0f)
        {
            horizontalAngle -= 360.0f;
        }
        if (horizontalAngle < -360.0f)
        {
            horizontalAngle += 360.0f;
        }
        // make vertical angle be between -LOOK_LIMIT and LOOK_LIMIT
        verticalAngle = Mathf.Clamp(verticalAngle, -LOOK_LIMIT, LOOK_LIMIT);
    }
}
