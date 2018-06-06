using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Excavation : MonoBehaviour {

    public float excavationSpeed;
    public float excavationReach;

    public GameObject excavator;
    public Terrain seafloor;

    private const int EXCAVATE_SIZE = 15;
    private const int EXCAVATE_SIZE_2 = EXCAVATE_SIZE / 2;

    // Use this for initialization
    void Start () {}
	
	// Update is called once per frame
	void Update ()
    {
        // EXCAVATION
        if (Input.GetButton("Fire1") || Input.GetButton("Fire2"))
        {
            // decide whether adding or subtracting sand
            float modifier = 1.0f;
            if (Input.GetButton("Fire2"))
            {
                modifier = -1.0f;
            }

            // do the ray cast to see where excavating
            RaycastHit hit;
            Ray ray = new Ray(excavator.transform.position, excavator.transform.TransformDirection(Vector3.up));
            if (Physics.Raycast(ray, out hit, excavationReach))
            {
                Debug.Log("ray cast hit at" + hit.point + " which is at uv " + hit.textureCoord + " and terrain has size " + seafloor.terrainData.size + " and heightmap width " + seafloor.terrainData.heightmapWidth);

                // calculate the point where the ray hit by using UV coordinates and heightmap resolution
                Vector3 hitpoint = new Vector3
                {
                    x = (hit.textureCoord.x * seafloor.terrainData.heightmapWidth),
                    y = 1.0f,
                    z = (hit.textureCoord.y * seafloor.terrainData.heightmapWidth)
                };

                // get the height samples from the right spot
                float[,] samples = seafloor.terrainData.GetHeights(
                    Mathf.CeilToInt(hitpoint.x)-EXCAVATE_SIZE_2,
                    Mathf.CeilToInt(hitpoint.z)-EXCAVATE_SIZE_2,
                    EXCAVATE_SIZE,
                    EXCAVATE_SIZE);

                // compute the change in each height sample
                for (int i = 0; i < EXCAVATE_SIZE; i++)
                {
                    for (int j = 0; j < EXCAVATE_SIZE; j++)
                    {
                        // local sample coordinates in the patch, centered in the middle
                        int sample_x = i - EXCAVATE_SIZE_2;
                        int sample_y = j - EXCAVATE_SIZE_2;

                        // use quadratic function to get amount of change in this sample
                        float a = 1.5f;
                        float b = a;
                        float c = 1.0f / EXCAVATE_SIZE_2;
                        float z = ((sample_x * sample_x * c) / (a * a)) + ((sample_y * sample_y * c) / (b * b));

                        float sample_amount = Mathf.Clamp(1.0f - z, 0.0f, 1.0f);

                        // update samples by combining sample change amount, speed, time, and switch for add/subtract
                        samples[i, j] = samples[i, j] + (sample_amount * modifier * excavationSpeed * Time.deltaTime);
                    }
                }

                // update the height samples
                seafloor.terrainData.SetHeights(
                    Mathf.CeilToInt(hitpoint.x)-EXCAVATE_SIZE_2,
                    Mathf.CeilToInt(hitpoint.z)-EXCAVATE_SIZE_2,
                    samples);
            }
        }
    }
}
