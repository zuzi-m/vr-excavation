using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeCamera : MonoBehaviour {

    public float speed;
    public Terrain Ter;

    private float horizontalAngle;
    private float verticalAngle;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        horizontalAngle += Input.GetAxis("Mouse X") * speed;
        verticalAngle += Input.GetAxis("Mouse Y") * speed;

        constrainCamera();
        Quaternion q = Quaternion.AngleAxis(horizontalAngle, Vector3.up) * Quaternion.AngleAxis(verticalAngle, -Vector3.right);

        transform.rotation = q;

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        transform.Translate(new Vector3(h, 0.0f, v));

        // EXCAVATION
        if (Input.GetButtonDown("Fire1"))
        {
            Debug.Log("fire pressed");
            RaycastHit hit;

            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit))
            {
                Debug.Log("ray cast hit at" + hit.point + " which is at uv " + hit.textureCoord + " and texture has this " + Ter.terrainData.detailWidth);
                int SIZ = 15;
                int SIZ2 = 7;
                float maxRad = Mathf.Sqrt(2 * (SIZ2 * SIZ2));

                Vector3 hitpoint = hit.point - Ter.transform.position;

                float[,] samples = Ter.terrainData.GetHeights(Mathf.CeilToInt(hitpoint.x), Mathf.CeilToInt(hitpoint.z), SIZ, SIZ);
                for (int i = 0; i < SIZ; i++)
                {
                    for (int j = 0; j < SIZ; j++)
                    {
                        float radius = Mathf.Sqrt(Mathf.Pow(Mathf.Abs(i - (SIZ / 2)), 2) + Mathf.Pow(Mathf.Abs(j - (SIZ / 2)), 2));
                        radius = Mathf.Abs(radius - maxRad) / maxRad;
                        samples[i, j] = samples[i, j] + 0.01f * radius;

                        //samples[i, j] = samples[i, j] + 0.1f * Time.deltaTime;
                    }
                }

                Ter.terrainData.SetHeights(Mathf.CeilToInt(hitpoint.x), Mathf.CeilToInt(hitpoint.z), samples);
            }
        }
    }

    void constrainCamera()
    {
        if(horizontalAngle > 360.0f)
        {
            horizontalAngle -= 360.0f;
        }
        if(horizontalAngle < -360.0f)
        {
            horizontalAngle += 360.0f;
        }
        Mathf.Clamp(verticalAngle, -45.0f, 45.0f);
    }
}
